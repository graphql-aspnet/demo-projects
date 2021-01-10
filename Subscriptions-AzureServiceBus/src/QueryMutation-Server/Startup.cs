namespace GraphQL.AspNet.Examples.Subscriptions.QueryMutationServer
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
    using System.Diagnostics;
    using GraphQL.AspNet.Schemas;
    using MutationSubscription_Server.Controllers;

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

            // Add a custom Subscription Event publisher
            // to the DI container before calling GraphQL
            //
            // *******************************************************
            // NOTE: for this example to work you must have your own
            // Azure Service Bus namespace with an appropriate topic and key created
            // *******************************************************
            services.AddSingleton<ISubscriptionEventPublisher>((sp) =>
            {
                var key = this.Configuration["AzureServiceBusConnectionString"];
                var topic = this.Configuration["AzureServiceBusTopic"];

                if (string.IsNullOrWhiteSpace(key))
                    throw new SchemaConfigurationException("A valid Azure Service Bus Key must be supplied");

                if (string.IsNullOrWhiteSpace(topic))
                    throw new SchemaConfigurationException("A valid Azure Service Bus Topic must be supplied");

                var logFactory = sp.GetService<ILoggerFactory>();
                return new AzureServiceBusSubscriptionPublisher(logFactory, key, topic);
            });

            // Note that instead of the general "AddSubscriptions"
            // we are using "AddSubscriptionPublishing" this only registers
            // the "publishing abilities" of the library. The publisher will be automatically
            // created from the registered type in the DI container set above as one that
            // publishes to Azure Service Bus
            //
            // By only registering the publishing abilities this server instance
            // will retain its default authorization method for any queries and mutations
            services.AddGraphQL(options =>
            {
                // register all controllers found in the shared assembly
                // so as to complete the full introspection documentation
                options.AddGraphAssembly(typeof(WidgetQueryController).Assembly);
            })
                .AddSubscriptionPublishing();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseGraphQL();

            if (Debugger.IsAttached)
            {
                Console.WriteLine("*****************************************");
                Console.WriteLine("** Query/Mutation Server Started");
                Console.WriteLine("**");
                Console.WriteLine("** GraphQL tools should be ");
                Console.WriteLine("** pointed to:  http://localhost:5000/graphql");
                Console.WriteLine("**");
                Console.WriteLine("*****************************************");
            }
        }
    }
}
