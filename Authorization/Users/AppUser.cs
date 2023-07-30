namespace GraphQL.AspNet.Examples.Authorization.Users
{
    public class AppUser
    {
        public AppUser(string id, string username)
        {
            this.Id = id;
            this.UserName = username;
            this.NormalizedUsername = username?.ToLower();
        }

        public string Id { get; }

        public string UserName { get; set; }

        public string NormalizedUsername { get; set; }
    }
}