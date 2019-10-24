namespace GraphQL.AspNet.Examples.Authorization.Users
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Primitives;

    public class IdBasedAuthOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "id-based-authentication";
        public string Scheme => DefaultScheme;
    }
}