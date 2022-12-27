namespace GraphQL.AspNet.Examples.Subscriptions.DataModel.Controllers
{
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;
    using GraphQL.AspNet.Examples.Subscriptions.DataModel;
    using GraphQL.AspNet.Interfaces.Controllers;
    using System.Collections.Generic;

    /// <summary>
    /// A sample controller to extract some widget data from the mock repository. There
    /// is nothing special about this controller related to subscriptions, it exists
    /// just to view data as a graphql query.
    /// </summary>
    public class WidgetQueryController : GraphController
    {
        private WidgetRepository _repo;

        public WidgetQueryController()
        {
            _repo = new WidgetRepository();
        }

        [QueryRoot("widget", typeof(Widget))]
        public IGraphActionResult RetrieveWidget(int id)
        {
            return this.Ok(_repo.FindById(id));
        }

        [QueryRoot("searchByName", typeof(IEnumerable<Widget>))]
        public IGraphActionResult SearchByName(string nameLike = "*")
        {
            return this.Ok(_repo.SearchByName(nameLike));
        }

        [QueryRoot("searchByDescription", typeof(IEnumerable<Widget>))]
        public IGraphActionResult SearchByDescription(string descriptionLike = "*")
        {
            return this.Ok(_repo.SearchByDescription(descriptionLike));
        }
    }
}