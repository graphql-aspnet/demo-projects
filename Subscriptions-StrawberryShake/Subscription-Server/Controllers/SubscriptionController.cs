namespace GraphQL.AspNet.Examples.StrawberryShakeServer.Controllers
{
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;
    using GraphQL.AspNet.Examples.StrawberryShakeServer.Models;
    using GraphQL.AspNet.Interfaces.Controllers;

    /// <summary>
    /// A simple controller that allows clients to subscribe and receive the current time 
    /// of the server.
    /// </summary>
    public class SubscriptionController : GraphController
    {
        public const string TIME_CHANGED_EVENT = "TIME_CHANGED";

        /// <summary>
        /// A subscription that relays the time value whenever its raised to the server.
        /// </summary>
        /// <param name="eventData">The data package raised with the event.</param>
        /// <returns></returns>
        [SubscriptionRoot("onTimeChanged", typeof(CurrentTimeEventData), EventName = TIME_CHANGED_EVENT)]
        public IGraphActionResult OnTimeChanged(CurrentTimeEventData eventData)
        {
            // relay the event data to each connected client.
            return this.Ok(eventData);
        }
    }
}
