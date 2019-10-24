namespace GraphQL.AspNet.Examples.Authorization.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;
    using GraphQL.AspNet.Examples.Authorization.Model;
    using Microsoft.AspNetCore.Authorization;

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

        /// <summary>
        /// Retrieves an entire list of donuts in the bakery
        /// </summary>
        [Authorize]
        [QueryRoot("allDonuts")]
        [Description("Retrieves all donuts from the system. This method requires an authorized user.")]
        public Task<IEnumerable<Donut>> RetrieveAllDonuts()
        {
            return Task.FromResult(new List<Donut>() {
                new Donut()
                {
                    Id = 5,
                    Name = "Super Mega Donut",
                    Flavor = DonutFlavor.Glazed,
                },
                new Donut()
                {
                    Id = 6,
                    Name = "Regular Donut",
                    Flavor = DonutFlavor.Chocolate,
                },
                new Donut()
                {
                    Id = 7,
                    Name = "Donut Hole",
                    Flavor = DonutFlavor.Sugar,
                }
                } as IEnumerable<Donut>);
        }
    }
}