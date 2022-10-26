namespace GraphQL.AspNet.Examples.ReactApollo
{
    using System;
    using GraphQL.AspNet.Configuration.Mvc;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.WebSockets;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        private const string ALL_ORIGINS_POLICY = "_allOrigins";
        private static TimeSpan KEEP_ALIVE_INTERVAL = TimeSpan.FromMilliseconds(10000);

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // apply an unrestricted cors policy for the demo services
            // to allow use on many of the tools for testing (graphiql, altair etc.)
            // Do not do this in production
            services.AddCors(options =>
            {
                options.AddPolicy(
                    ALL_ORIGINS_POLICY,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });


            services.AddWebSockets((options) =>
            {
                // add some common origins of various tools that may be
                // used for running this demo
                // do not add these in a production app
                options.AllowedOrigins.Add("http://localhost:5000");
                options.AllowedOrigins.Add("http://localhost:4000");
                options.AllowedOrigins.Add("http://localhost:3000");

                // sent by some electron-based graphql tools
                options.AllowedOrigins.Add("file://");

                // some graphql tools have a very low threshold for disconnecting a subscription
                // when idle.  Set the server-sent keep alive value to a low interval
                // to force them to keep the connection alive.
                // in production the value you would use would be dependent on your needs.
                options.KeepAliveInterval = KEEP_ALIVE_INTERVAL;
            });

            services.AddControllers();
            services.AddGraphQL()
                    .AddSubscriptions(options =>
                    {
                        options.AuthenticatedRequestsOnly = false;

                        // some graphql tools have a very low threshold for disconnecting a subscription
                        // when idle.  Set the server-sent keep alive value to a low interval
                        // to force them to keep the connection alive.
                        // in production the value you would use would be dependent on your needs.
                        options.ConnectionKeepAliveInterval = KEEP_ALIVE_INTERVAL;
                    });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseCors(ALL_ORIGINS_POLICY);

            app.UseWebSockets();

            app.UseGraphQL();
        }
    }
}