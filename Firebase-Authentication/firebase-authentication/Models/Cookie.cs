namespace Firebase.AuthTest.Models
{
    using GraphQL.AspNet.Attributes;

    /// <summary>
    /// A scrumptious cookie!
    /// </summary>
    public class Cookie
    {
        public Cookie(string name, string flavor)
        {
            this.Name = name;
            this.Flavor = flavor;
        }

        /// <summary>
        /// Clones the cookie and assigns the id of the user this new cookie was
        /// specifically made for.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Cookie.</returns>
        [GraphSkip]
        public Cookie MadeFor(string userId)
        {
            var clone = new Cookie(this.Name, this.Flavor);
            clone.Owner = userId;
            return clone;
        }

        public string Owner { get; private set; }

        public string Name { get; }

        public string Flavor { get; }
    }
}
