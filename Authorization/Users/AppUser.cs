namespace GraphQL.AspNet.Examples.Authorization.Users
{
    using Microsoft.AspNet.Identity;

    public class AppUser : IUser<string>
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