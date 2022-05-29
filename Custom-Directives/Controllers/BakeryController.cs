namespace GraphQL.AspNet.Examples.CustomDirectives.Controllers
{
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Controllers;
    using GraphQL.AspNet.Examples.CustomDirectives.Model;
    using System.ComponentModel;
    using System.Threading.Tasks;

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
            // NOTE: The donut created here has a standard casing
            // Name field. But when queried the name is upper case due to
            // the @toUpper directive applied to the field definition during
            // startup
            //
            // additionally the @toSarcastic directive can be supplied in a query
            // document, furhter altering the value of the name field
            return Task.FromResult(new Donut()
            {
                Id = id,
                Name = "Regular Donut",
                Flavor = DonutFlavor.Chocolate,
            });
        }
    }
}