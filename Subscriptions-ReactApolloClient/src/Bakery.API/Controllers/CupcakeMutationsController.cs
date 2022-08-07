namespace GraphQL.AspNet.Examples.ReactApollo.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;
    using GraphQL.AspNet.Examples.ReactApollo.Model;
    using GraphQL.AspNet.Interfaces.Controllers;

    [GraphRoot]
    public class CupcakeMutationsController : GraphController
    {
        private CupcakeService _cupcakeService;

        public CupcakeMutationsController()
        {
            _cupcakeService = new CupcakeService();
        }

        [Mutation("updateCupcake", typeof(bool))]
        public IGraphActionResult UpdateCupcake(Cupcake cupcake)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var updatedCake = _cupcakeService.UpdateCupcake(cupcake.Id ?? -1, cupcake.Name, cupcake.Flavor);
            if (updatedCake == null)
                return this.BadRequest("Cupcake not found");

            this.PublishSubscriptionEvent(PastryConstants.CUPCAKE_UPDATED, updatedCake);
            return this.Ok(true);
        }

        [Mutation("addCupcake", typeof(bool))]
        public IGraphActionResult AddCupcake(Cupcake cupcake)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            var updatedCake = _cupcakeService.CreateCupcake(cupcake.Name, cupcake.Flavor, cupcake.Quantity);
            if (updatedCake == null)
                return this.BadRequest("Error creating the new cupcake");

            this.PublishSubscriptionEvent(PastryConstants.CUPCAKE_CREATED, updatedCake);
            return this.Ok(true);
        }

        [Mutation("purchaseCupcake", typeof(bool))]
        public IGraphActionResult SellCupcake(int id)
        {
            var sold = _cupcakeService.SellSingleCupcake(id);

            if (sold)
            {
                var cupcake = _cupcakeService.RetrieveCupCake(id);
                this.PublishSubscriptionEvent(PastryConstants.CUPCAKE_SOLD, cupcake);
            }

            return this.Ok(sold);
        }
    }
}
