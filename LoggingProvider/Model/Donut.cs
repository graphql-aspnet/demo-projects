namespace GraphQL.AspNet.Examples.LoggingProvider.Model
{
    using System.ComponentModel;

    /// <summary>
    /// A representation of a donut type sold by a bakery.
    /// </summary>
    [Description("A donut type that the bakery is capable of making")]
    public class Donut
    {
        /// <summary>
        /// Gets or sets the unique id of this donut.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of this donut.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the flavor of this particular donut.
        /// </summary>
        /// <value>The flavor.</value>
        public DonutFlavor Flavor { get; set; }
    }
}