namespace GraphQL.AspNet.Examples.Subscriptions.DataModel.Controllers
{
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;
    using GraphQL.AspNet.Examples.Subscriptions.DataModel;
    using GraphQL.AspNet.Interfaces.Controllers;

    /// <summary>
    /// A controller containing a method to update a widget's data in the "datbase".
    /// This controller contains the meat of this example project and demonstrates how to
    /// "raise subscription events" such that any listening subscribers (to the subscription server)
    /// would receive them.
    /// </summary>
    public class WidgetMutationsController : GraphController
    {
        private WidgetRepository _repo;

        public WidgetMutationsController()
        {
            _repo = new WidgetRepository();
        }

        /// <summary>
        /// Updates the "database" that the supplied widget was changed
        /// and notifies any listening subscriptions of the change.
        /// </summary>
        /// <param name="widgetData">The widget data.</param>
        /// <returns>IGraphActionResult.</returns>
        [MutationRoot("updateWidget", typeof(Widget))]
        public IGraphActionResult UpdateWidget(Widget widgetData)
        {
            // validation
            if (!ModelState.IsValid)
                return this.BadRequest(ModelState);

            // update the "database" with the requested changes
            _repo.InsertOrUpdateWidget(widgetData);

            // *********************************************
            // raise the event indicating that the widget was updated.
            // If you don't call this message, no registered subscriptions
            // will be notified.
            // *********************************************
            this.PublishSubscriptionEvent(
                WidgetConstants.WIDGET_INSERTED_OR_ADDED,
                widgetData);

            // return
            return this.Ok(widgetData);
        }
    }
}