namespace Firebase.AuthTest.Models
{
    using GraphQL.AspNet.Attributes;

    /// <summary>
    /// A yummy donut!.
    /// </summary>
    public class Donut
    {
        public Donut(string name, string flavor)
        {
            this.Name = name;
            this.Flavor = flavor;
        }

        /// <summary>
        /// Clones the donut and assigns the id of the user this new donut was
        /// specifically made for.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Donut.</returns>
        [GraphSkip]
        public Donut MadeFor(string userId)
        {
            var clone = new Donut(this.Name, this.Flavor);
            clone.Owner = userId;
            return clone;
        }

        public string Owner { get; private set; }

        public string Name { get; }

        public string Flavor { get; }
    }
}