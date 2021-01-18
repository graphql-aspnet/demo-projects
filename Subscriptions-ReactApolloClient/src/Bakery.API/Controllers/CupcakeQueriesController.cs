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

    [GraphRoute("cupcakes")]
    public class CupcakeQueriesController : GraphController
    {
        private CupcakeService _cupcakeService;

        public CupcakeQueriesController()
        {
            _cupcakeService = new CupcakeService();
        }

        [Query("search", typeof(IEnumerable<Cupcake>))]
        public IGraphActionResult SearchCupcakes(string nameLike = "*", PastryFlavor? flavor = null)
        {
            return this.Ok(_cupcakeService.SearchCupCakes(nameLike, flavor));
        }

        [QueryRoot("cupcake", typeof(Cupcake))]
        public IGraphActionResult RetrieveCupcake(int id)
        {
            return this.Ok(_cupcakeService.RetrieveCupCake(id));
        }
    }
}
