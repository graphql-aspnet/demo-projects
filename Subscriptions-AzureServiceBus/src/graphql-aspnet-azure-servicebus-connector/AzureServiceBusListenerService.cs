namespace GraphQL.AspNet.AzureServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure.Messaging.ServiceBus;
    using GraphQL.AspNet.AzureServiceBus.Serializers;
    using GraphQL.AspNet.Common;
    using GraphQL.AspNet.Execution.Subscriptions;
    using GraphQL.AspNet.Interfaces.Subscriptions;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A "hosted service" in the asp.net environment that continuously listens for
    /// messages on a service bus and forwards then into the graphql-aspnet
    /// </summary>
    public class GraphQLAzureServiceBusListenerService : BackgroundService
    {
        // ****************************************************************************
        // Note: this demo connector does not attempt to address common service bus
        //       concerns such as message batching, failed sending, retries,
        //       message duplication, serialization failures etc.
        // ****************************************************************************

        private readonly string _cnnString;
        private readonly string _topic;
        private readonly string _subscription;
        private readonly ILogger _logger;
        private readonly ISubscriptionEventRouter _router;
        private JsonSerializerOptions _serializationOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureServiceBusSubscriptionListener" /> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="router">The internal router to which deserialzied messages
        /// should be pushed.</param>
        /// <param name="cnnString">The CNN string.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="subscriptionName">Name of the subscription.</param>
        public GraphQLAzureServiceBusListenerService(
            ILoggerFactory loggerFactory,
            ISubscriptionEventRouter router,
            string cnnString,
            string topic,
            string subscriptionName)
        {
            _logger = loggerFactory?.CreateLogger("GraphQLAzureServiceBusDemo");
            _router = Validation.ThrowIfNullOrReturn(router, nameof(router));
            _cnnString = Validation.ThrowIfNullWhiteSpaceOrReturn(cnnString, nameof(cnnString), false);
            _topic = Validation.ThrowIfNullWhiteSpaceOrReturn(topic, nameof(topic), false);
            _subscription = Validation.ThrowIfNullWhiteSpaceOrReturn(subscriptionName, nameof(subscriptionName), false);

            _serializationOptions = new JsonSerializerOptions();
            _serializationOptions.PropertyNameCaseInsensitive = true;
            _serializationOptions.WriteIndented = true;
            _serializationOptions.Converters.Add(new SubscriptionEventConverter());
        }

        /// <summary>
        /// Begin the hosted service and start watching for bus messages
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the long running operations.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var options = new ServiceBusReceiverOptions();

            var client = new ServiceBusClient(_cnnString);
            var reciever = client.CreateReceiver(
                _topic,
                _subscription,
                options);

            _logger?.LogInformation($"ASB Monitor Starting. Topic: {_topic}, Subscription: {_subscription}");

            // Begin listening to the bus topic for any new messages.
            // The use of a 2 second max wait time is arbitrary in this case
            // and for demo purposes only.
            // Your use case will likely be different.
            var maxWait = TimeSpan.FromSeconds(2);
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = await reciever.ReceiveMessageAsync(maxWait, stoppingToken);
                if (message != null)
                {
                    _logger?.LogDebug($"ASB Message Recieved: {message.MessageId}");

                    this.ProcessMessage(message, stoppingToken);
                    await reciever.CompleteMessageAsync(message);

                    _logger?.LogDebug($"ASB Message Completed: {message.MessageId}");
                }
            }
        }

        private void ProcessMessage(ServiceBusReceivedMessage message, CancellationToken cancelToken = default)
        {
            // deserialize the message back into a Subscription event
            var messageBody = message.Body.ToString();
            var subscriptionEvent = JsonSerializer.Deserialize<SubscriptionEvent>(messageBody, _serializationOptions);
            if (subscriptionEvent == null)
                return;

            // forward the event into the local router so it can be processed by any
            // listening subscription servers (potentially 1 per hosted schema).
            _router.RaisePublishedEvent(subscriptionEvent);
        }
    }
}
