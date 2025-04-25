namespace Subscription_Server
{
    using System;
    using GraphQL.AspNet.Configuration;
    using GraphQL.AspNet.Examples.StrawberryShakeServer.BackgroundServices;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.WebSockets;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        private const string ALL_ORIGINS_POLICY = "_allOrigins";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Set up cors to allow anything through
            // For demo purposes only. Don't do this in a real application.
            // -------------------------------------------------
            builder.Services.AddCors(options =>
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

            builder.Services.AddWebSockets(options =>
            {
                // server address
                options.AllowedOrigins.Add("http://localhost:5000");
                options.AllowedOrigins.Add("ws://localhost:5000");

                // here add some common origins of various tools that may be
                // used for running this demo
                // do not add these in a production app
                options.AllowedOrigins.Add("null");

                // some electron-based graphql tools send a file reference
                // as their origin
                // do not add these in a production app
                options.AllowedOrigins.Add("file://");
                options.AllowedOrigins.Add("ws://");
            });

            // Setup Graph QL
            // -------------------------------------------------
            builder.Services
                .AddGraphQL()
                .AddSubscriptions();

            // Add the background service that will continuously raise 
            // event data about the time of day.
            builder.Services.AddHostedService<TimeEventPublisher>();

            // Configure the HTTP request pipeline.
            var app = builder.Build();

            app.UseCors(ALL_ORIGINS_POLICY);

            // make sure we can accept web socket connections
            app.UseWebSockets();

            // handle graphql requests
            app.UseGraphQL();

            app.MapGet("/", () => "Hello World!");
            app.Run();
        }
    }
}
