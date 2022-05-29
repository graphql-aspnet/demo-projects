namespace GraphQL.AspNet.Examples.CustomDirectives.Directives
{
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Directives;
    using GraphQL.AspNet.Execution;
    using GraphQL.AspNet.Execution.Contexts;
    using GraphQL.AspNet.Interfaces.Controllers;
    using GraphQL.AspNet.Interfaces.TypeSystem;
    using GraphQL.AspNet.Schemas.TypeSystem;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A type system directive that can be applied to any string based FIELD_DEFINITION.
    /// This directive will take any resolved string value and convert it to an
    /// UPPER CASE string.
    /// </summary>
    public class ToUpperDirective : GraphDirective
    {
        /// <summary>
        /// Defines the action to take when the directive is applied to a
        /// FIELD_DEFINITION location is the type system.
        /// </summary>
        /// <returns>IGraphActionResult.</returns>
        [DirectiveLocations(DirectiveLocation.FIELD_DEFINITION)]
        public IGraphActionResult UpdateResolver()
        {
            // cast the directive's current target (ISchemaItem) into
            // the graph field we expect it to be
            if (this.DirectiveTarget is IGraphField gf)
            {
                if (gf.ObjectType != typeof(string))
                {
                    throw new System.Exception(
                        "The ToUpper directive can only " +
                        "extend fields that return a string."); // - hulk
                }

                // create a new resolver that executes the orignal
                // resolver then "upper cases" any string result
                //
                // .Extend is an extension method that can be applied to
                // any resolver.
                //
                // Resolvers can be extended as many times as you want to
                // chain additional functionality
                var resolver = gf.Resolver.Extend(ConvertToupper);

                // set this new resolver as the resolver of the field
                gf.UpdateResolver(resolver);
            }

            return Ok();
        }

        /// <summary>
        /// Converts the result of the field to an UPPER CASE string.
        /// </summary>
        /// <param name="context">The resolution context, at query time, that is being resolved.</param>
        /// <param name="token">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Task.</returns>
        private static Task ConvertToupper(FieldResolutionContext context, CancellationToken token)
        {
            // Field resolvers are given the entire resolution context to work with
            // Here we update the Result property to be the new upper case value
            //
            // Be sure to account for possible null values in the case of a failed
            // resolution or no data being available.
            if (context.Result != null)
                context.Result = context.Result.ToString().ToUpper();

            return Task.CompletedTask;
        }
    }
}