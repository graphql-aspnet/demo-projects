namespace GraphQL.AspNet.Examples.StrawberryShakeServer.BackgroundServices
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using GraphQL.AspNet.Examples.StrawberryShakeServer.Controllers;
    using GraphQL.AspNet.Examples.StrawberryShakeServer.Models;
    using GraphQL.AspNet.Interfaces.Subscriptions;
    using GraphQL.AspNet.Schemas;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A simple hosted service that runs in the background on the server continually rasing the time of 
    /// day each second as it changes.
    /// </summary>
    internal sealed class TimeEventPublisher : BackgroundService
    {
        private readonly ISubscriptionEventRouter _eventRouter;
        private readonly ILogger<TimeEventPublisher> _logger;

        public TimeEventPublisher(ISubscriptionEventRouter eventRouter, ILogger<TimeEventPublisher> logger)
        {
            _eventRouter = eventRouter;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger?.LogInformation("Time Publisher started.");
            var currentTime = TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(Math.Round(DateTime.UtcNow.TimeOfDay.TotalSeconds)));
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(500, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }

                var now = TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(Math.Round(DateTime.UtcNow.TimeOfDay.TotalSeconds)));
                if (currentTime != now)
                {
                    var eventData = new CurrentTimeEventData
                    {
                        CurrentTime = now,
                    };
                    RaisePublishedEvent(eventData, SubscriptionController.TIME_CHANGED_EVENT);
                    _logger.LogDebug($"Time Change Published: {now:HH:mm:ss}");
                }

                currentTime = now;
            }
        }

        /// <summary>
        /// When the time of day changes
        /// </summary>
        /// <param name="eventData">The data package to publish</param>
        /// <param name="eventName">The name of the event to publish teh data package as</param>
        private void RaisePublishedEvent(CurrentTimeEventData eventData, string eventName)
        {
            var subscriptionEvent = new GraphQL.AspNet.SubscriptionServer.SubscriptionEvent
            {
                Id = Guid.NewGuid().ToString(),
                EventName = eventName,
                Data = eventData,
                SchemaTypeName = typeof(GraphSchema).AssemblyQualifiedName,
                DataTypeName = typeof(CurrentTimeEventData).AssemblyQualifiedName,
            };

            _eventRouter?.RaisePublishedEvent(subscriptionEvent);
        }
    }
}
