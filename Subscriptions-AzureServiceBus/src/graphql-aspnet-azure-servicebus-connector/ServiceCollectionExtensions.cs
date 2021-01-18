namespace GraphQL.AspNet.AzureServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using GraphQL.AspNet.Configuration.Exceptions;
    using GraphQL.AspNet.Interfaces.Subscriptions;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A set of methods to extend and add functionality to <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the GraphQL Azure Service Bus Listener as a hosted service within the application
        /// domain.
        /// </summary>
        /// <param name="services">The services collection to register to.</param>
        /// <param name="serviceBusConnectionString">The connection string to a service bus namespace.</param>
        /// <param name="topic">The topic on the namespace to recieve messages from.</param>
        /// <param name="subscription">The subscription within the topic to listen on.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddGraphQLAzureServiceBusListener(
            this IServiceCollection services,
            string serviceBusConnectionString,
            string topic,
            string subscription)
        {
            // register the listener as a background service hosted in this aspnet application
            // the listener will contiuously monitor the service bus topic using the given subscription
            // for new messages
            services.AddHostedService<GraphQLAzureServiceBusListenerService>((sp) =>
            {
                if (string.IsNullOrWhiteSpace(serviceBusConnectionString))
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

                return new GraphQLAzureServiceBusListenerService(logFactory, router, serviceBusConnectionString, topic, subscription);
            });

            return services;
        }
    }
}
