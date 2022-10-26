namespace GraphQL.AspNet.AzureServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure.Messaging.ServiceBus;
    using GraphQL.AspNet.AzureServiceBus.Serializers;
    using GraphQL.AspNet.Common;
    using GraphQL.AspNet.Execution.Subscriptions;
    using GraphQL.AspNet.Interfaces.Subscriptions;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A bare bones, custom publisher that is used by the graphql-aspnet library to publish
    /// event data to an service bus such that it can be consumed by multiple
    /// subscription servers.
    /// </summary>
    public class AzureServiceBusSubscriptionPublisher : ISubscriptionEventPublisher
    {
        // ****************************************************************************
        // Note: this demo connector does not attempt to address common service bus
        //       concerns such as message batching, failed sending, retries,
        //       message duplication, serialization failures etc.
        // ****************************************************************************

        private string _cnnString;
        private string _topic;
        private ILogger _logger;
        private JsonSerializerOptions _serializationOptions;

        public AzureServiceBusSubscriptionPublisher(ILoggerFactory loggerFactory, string cnnString, string topic)
        {
            _cnnString = Validation.ThrowIfNullWhiteSpaceOrReturn(cnnString, nameof(cnnString), false);
            _topic = Validation.ThrowIfNullWhiteSpaceOrReturn(topic, nameof(topic), false);
            _logger = loggerFactory?.CreateLogger("GraphQLAzureServiceBusDemo");

            _serializationOptions = new JsonSerializerOptions();
            _serializationOptions.PropertyNameCaseInsensitive = true;
            _serializationOptions.WriteIndented = true;
            _serializationOptions.Converters.Add(new SubscriptionEventConverter());
        }

        /// <summary>
        /// Raises a new event in a manner such that a compatible <see cref="ISubscriptionEventRouter" /> could
        /// receive it for processing.
        /// </summary>
        /// <param name="eventData">The event to publish.</param>
        /// <param name="cancelToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Task.</returns>
        public async ValueTask PublishEvent(SubscriptionEvent eventData, CancellationToken cancelToken = default)
        {
            Validation.ThrowIfNull(eventData, nameof(eventData));

            await using (ServiceBusClient sbClient = new ServiceBusClient(_cnnString))
            {
                var sender = sbClient.CreateSender(_topic);

                // Serialization of data objects (those contained within the event)
                // can be complex. There is no one correct method for data object serialization
                // and you'll likely need to write your own converters to facilitate the process
                var serializedData = JsonSerializer.Serialize(
                    eventData,
                    typeof(SubscriptionEvent),
                    _serializationOptions);

                var message = new ServiceBusMessage(serializedData);
                await sender.SendMessageAsync(message);

                _logger?.LogDebug($"ASB Message Published: Topic: {_topic}");
            }
        }
    }
}
