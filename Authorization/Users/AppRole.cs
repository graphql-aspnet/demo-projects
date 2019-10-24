namespace GraphQL.AspNet.Examples.Authorization.Users
{
    using System;
    using Microsoft.AspNet.Identity;

    public class AppRole : IRole<string>
    {
        public AppRole(string id, string name)
        {
            this.Id = id;
            this.Name = name;
            this.NormalizedName = name?.ToLower();
        }

        public string Id { get; }

        public string Name { get; set; }
        public string NormalizedName { get; set; }
    }
}