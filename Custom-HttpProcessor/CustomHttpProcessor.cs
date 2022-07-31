namespace GraphQL.AspNet.Examples.CustomHttpProcessor
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using GraphQL.AspNet.Defaults;
    using GraphQL.AspNet.Interfaces.Engine;
    using GraphQL.AspNet.Interfaces.Logging;
    using GraphQL.AspNet.Interfaces.Middleware;
    using GraphQL.AspNet.Middleware.QueryExecution;
    using GraphQL.AspNet.Schemas;

    /// <summary>
    /// A custom Http Processor that recieves the raw <see cref="HttpContext"/> and intercepts the query
    /// denying it while donuts are being made :)
    /// </summary>
    public class CustomHttpProcessor : DefaultGraphQLHttpProcessor<GraphSchema>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomHttpProcessor"/> class.
        /// </summary>
        /// <param name="schema">The schema instance.</param>
        /// <param name="graphqlRunTime">The graphql runtime for the schema.</param>
        /// <param name="writer">The serializer to turn a response into json.</param>
        /// <param name="logger">The logger instance to use for recording events.</param>
        public CustomHttpProcessor(
            GraphSchema schema,
            IGraphQLRuntime<GraphSchema> graphqlRunTime,
            IGraphResponseWriter<GraphSchema> writer,
            IGraphEventLogger logger = null)
            : base(schema, graphqlRunTime, writer, logger)
        {
        }

        /// <summary>
        /// Submits the GraphQL query for processing.
        /// </summary>
        /// <param name="queryData">The query data.</param>
        /// <param name="cancelToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        public override Task SubmitGraphQLQuery(GraphQueryData queryData, CancellationToken cancelToken = default)
        {
            // Deny ALL graph ql requests from being sent to a client between 1am and 4am
            // Place a breakpoint here and/or alter the hours to see that its being invoked.
            if ((DateTime.UtcNow.Second % 2) == 0)
            {
                var response = this.ErrorMessageAsGraphQLResponse(
                    "This service denys all queries between on even seconds while we check the drive through lane.");

                return this.WriteResponse(response);
            }
            else
            {
                return base.SubmitGraphQLQuery(queryData);
            }
        }
    }
}