namespace GraphQL.AspNet.Examples.CustomHttpProcessor.Controllers
{
    using System.ComponentModel;
    using System.Threading.Tasks;
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;
    using GraphQL.AspNet.Examples.CustomHttpProcessor.Model;

    /// <summary>
    /// A controller centered around operations related to a mock bakery
    /// </summary>
    public class BakeryController : GraphController
    {
        /// <summary>
        /// Retrieves a single donut type from the bakery.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;Donut&gt;.</returns>
        [QueryRoot("donut")]
        [Description("Retrieves a single donut type from the bakery.")]
        public Task<Donut> RetrieveDonut(int id)
        {
            if (id == 5)
            {
                return Task.FromResult(new Donut()
                {
                    Id = 5,
                    Name = "Super Mega Donut",
                    Flavor = DonutFlavor.Glazed,
                });
            }
            else
            {
                return Task.FromResult(new Donut()
                {
                    Id = id,
                    Name = "Regular Donut",
                    Flavor = DonutFlavor.Chocolate,
                });
            }
        }
    }
}