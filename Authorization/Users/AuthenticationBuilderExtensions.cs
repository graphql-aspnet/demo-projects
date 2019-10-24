namespace GraphQL.AspNet.Examples.Authorization.Users
{
    using System;
    using Microsoft.AspNetCore.Authentication;

    public static class AuthenticationBuilderExtensions
    {
        // Custom authentication extension method
        public static AuthenticationBuilder AddIdBasedAuthentication(
            this AuthenticationBuilder builder)
        {
            // Add custom authentication scheme with custom options and custom handler
            return builder.AddScheme<IdBasedAuthOptions, IdBasedAuthHandler>(
                IdBasedAuthOptions.DefaultScheme,
                (o) => { });
        }
    }
}