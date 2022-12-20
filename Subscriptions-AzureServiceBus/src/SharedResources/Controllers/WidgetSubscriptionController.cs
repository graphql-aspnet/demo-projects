namespace GraphQL.AspNet.Examples.Subscriptions.DataModel.Controllers
{
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;
    using GraphQL.AspNet.Examples.Subscriptions.DataModel;
    using GraphQL.AspNet.Interfaces.Controllers;

    /// <summary>
    /// A sample controller exposing one subscription end point (at the root level)
    /// that will notify subscribers of changes to widgets within the system.
    /// </summary>
    public class WidgetSubscriptionController : GraphController
    {
        /// <summary>
        /// When a subscription is registered to this endpoint any widget updated
        /// via the query/mutation server is processed by this method for the
        /// registered subscription(s). If that subscription has indicated that it wishes
        /// to recieve the widget (via a match in the "nameLike" parameter) the widget
        /// is sent to that subscription.
        ///
        /// Note: The use of 'WidgetConstants.WIDGET_INSERTED_OR_UPDATED' value. This identifies
        ///       which "raised event" by a mutation cooripsonds should be listened for.
        /// </summary>
        /// <param name="eventData">The event data raised by the mutation server.</param>
        /// <param name="nameLike">The name text to filter on.</param>
        /// <returns>IGraphActionResult.</returns>
        [SubscriptionRoot("widgetChanged", typeof(Widget), EventName = WidgetConstants.WIDGET_INSERTED_OR_ADDED)]
        public IGraphActionResult WidgetChanged([SubscriptionSource] Widget eventData, string nameLike = "*")
        {
            // note on the parameters: [SubscriptionSource] is not required in most cases,
            //                         the library will usually be able to identify the correct parameter
            //                         as the event data source (it must match the type declaration in [SubscriptionRoot]).
            //
            //                         Also, the data type of the
            //                         [SubscriptionSource] parameter MUST match the dataObject
            //                         supplied in 'PublishSubscriptionEvent' raised during a mutation.
            //
            //
            //
            // for the purpose of this example, ignore any events raised with invalid data.
            // This could happen if a mutation calls PublishSubscriptionEvent
            // with bad data. A more robust error handling solution should be used
            // in a real application.
            if (!ModelState.IsValid)
                return this.Ok();

            // use the supplied nameLike parameter (supplied by the subscriber) to filter the data
            // and determine if the data should be sent to the listener
            if (nameLike == "*" || eventData.Name != null && eventData.Name.StartsWith(nameLike))
                return Ok(eventData);

            return this.Ok();
        }
    }
}