namespace GraphQL.AspNet.Examples.Authorization.Users
{
    public class AppRole
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