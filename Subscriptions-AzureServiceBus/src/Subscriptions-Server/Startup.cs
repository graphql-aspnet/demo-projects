namespace GraphQL.AspNet.Examples.Subscriptions.SubscriptionServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using GraphQL.AspNet.Configuration.Mvc;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using GraphQL.AspNet.Interfaces.Subscriptions;
    using GraphQL.AspNet.AzureServiceBus;
    using GraphQL.AspNet.Configuration.Exceptions;
    using GraphQL.AspNet.AzureServiceBus.Serializers;
    using System.Diagnostics;
    using GraphQL.AspNet.Examples.Subscriptions.DataModel;
    using Subscriptions_Server.Controllers;
    using Microsoft.AspNetCore.WebSockets;

    public class Startup
    {
        private const string ALL_ORIGINS_POLICY = "_allOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The services.</param>
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

            services.AddControllers();

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
            });

            // Adds a hosted listener that will monitor the service
            // bus for new messages and forward them into graphql-aspnet
            // for processing
            //
            // *******************************************************
            // NOTE: for this example to work you must have your own
            // Azure Service Bus namespace with an appropriate topic and key created
            // *******************************************************
            services.AddHostedService<AzureServiceBusListenerService>((sp) =>
            {
                var cnnString = this.Configuration["AzureServiceBusConnectionString"];
                var topic = this.Configuration["AzureServiceBusTopic"];
                var subscription = this.Configuration["AzureServiceBusSubscription"];

                if (string.IsNullOrWhiteSpace(cnnString))
                    throw new SchemaConfigurationException("A valid Azure Service Bus connection string must be supplied");

                if (string.IsNullOrWhiteSpace(topic))
                    throw new SchemaConfigurationException("A valid Azure Service Bus Topic must be supplied");

                if (string.IsNullOrWhiteSpace(subscription))
                    throw new SchemaConfigurationException("A valid Azure Service Bus subscription must be supplied");

                // the router is the link between external events
                // and local processing. It is an object to
                // which any subscription event can be sent and graphql-aspnet will
                // correctly route the event to any subscribers
                // for any registered schema. graphql-aspnet provides a default implementation
                // but it can be overriden if necessary (usually this is not required)
                var router = sp.GetService<ISubscriptionEventRouter>();
                var logFactory = sp.GetService<ILoggerFactory>();

                return new AzureServiceBusListenerService(logFactory, router, cnnString, topic, subscription);
            });

            // Note that instead of the general "AddSubscriptions"
            // we are using "AddSubscriptionServer" this only registers
            // the websocket management and event listener portions of the library
            // with our customer listener that is listening to an Azure service bus topic.
            services.AddGraphQL((options) =>
            {
                options.AddGraphAssembly(typeof(WidgetSubscriptionController).Assembly);
            })
            .AddSubscriptionServer(options =>
            {
                options.RequiredAuthenticatedConnection = false;
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(ALL_ORIGINS_POLICY);

            // enable web sockets on this server instance
            // this must be done before a call to 'UseGraphQL' if subscriptions are enabled for any
            // schema otherwise the subscriptions may not register correctly
            app.UseWebSockets();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseGraphQL();

            if (Debugger.IsAttached)
            {
                Console.WriteLine("*****************************************");
                Console.WriteLine("** Subscription Server Started");
                Console.WriteLine("**");
                Console.WriteLine("** GraphQL tools Subscription WS URL");
                Console.WriteLine("** should be pointed to:  ws://localhost:55000/graphql");
                Console.WriteLine("**");
                Console.WriteLine("*****************************************");
            }
        }
    }
}
