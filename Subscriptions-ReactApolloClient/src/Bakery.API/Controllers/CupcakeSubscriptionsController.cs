namespace GraphQL.AspNet.Examples.ReactApollo.Controllers
{
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;
    using GraphQL.AspNet.Examples.ReactApollo.Model;
    using GraphQL.AspNet.Interfaces.Controllers;
    using System;

    [GraphRoot]
    public class CupcakesSubscriptionsController : GraphController
    {
        [SubscriptionRoot("onCupcakeUpdated", typeof(Cupcake), EventName = PastryConstants.CUPCAKE_UPDATED)]
        public IGraphActionResult CupcakeUpdated(Cupcake eventData, string nameLike = "*")
        {
            if (this.IsMatch(eventData, nameLike))
                return this.Ok(eventData);

            return this.Ok();
        }

        [SubscriptionRoot("onCupcakeSold", typeof(Cupcake), EventName = PastryConstants.CUPCAKE_SOLD)]
        public IGraphActionResult CupcakeSold(Cupcake eventData, string nameLike = "*")
        {
            if (this.IsMatch(eventData, nameLike))
                return this.Ok(eventData);

            return this.Ok();
        }

        [SubscriptionRoot("onCupcakeCreated", typeof(Cupcake), EventName = PastryConstants.CUPCAKE_CREATED)]
        public IGraphActionResult NewCupcakeCreated(Cupcake eventData, string nameLike = "*")
        {
            if (this.IsMatch(eventData, nameLike))
                return this.Ok(eventData);

            return this.Ok();
        }

        private bool IsMatch(Cupcake eventData, string nameLike)
        {
            return nameLike == null || nameLike == "*" || eventData.Name.StartsWith(nameLike, StringComparison.OrdinalIgnoreCase);
        }
    }
}