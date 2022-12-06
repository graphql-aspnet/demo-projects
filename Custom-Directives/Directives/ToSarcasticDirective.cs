namespace GraphQL.AspNet.Examples.CustomDirectives.Directives
{
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Directives;
    using GraphQL.AspNet.Execution.Contexts;
    using GraphQL.AspNet.Interfaces.Controllers;
    using GraphQL.AspNet.Interfaces.Execution.QueryPlans.Document.Parts;
    using GraphQL.AspNet.Schemas.TypeSystem;
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// An execution directive that can be applied to any field in query document.
    /// When the resolved value of the field is a string convert that string to be
    /// in "SaRcAsTiC CaSiNg"
    /// </summary>
    public class ToSarcasticDirective : GraphDirective
    {
        [DirectiveLocations(DirectiveLocation.FIELD)]
        public IGraphActionResult Execute(bool startOnLowerCase = true)
        {
            // this directives targets a field in a query document
            // represented by the IFieldDocumentPart
            var fieldPart = this.DirectiveTarget as IFieldDocumentPart;
            if (fieldPart != null)
            {
                if (fieldPart.Field?.ObjectType != typeof(string))
                    throw new Exception("ONLY STRINGS!"); // - hulk

                // A post processor is an additional field resolver executed
                // immediately after the field's primary resolver finishes
                // executing
                fieldPart.PostResolver = CreateResolver(startOnLowerCase);
            }

            return this.Ok();
        }

        private static Func<FieldResolutionContext, CancellationToken, Task> CreateResolver(bool startOnLowerCase)
        {
            return (FieldResolutionContext context, CancellationToken token) =>
            {
                if (context.Result != null && context.Result is string)
                {
                    // for a single string leaf field convert and return
                    var data = context.Result.ToString();
                    var builder = new StringBuilder();

                    var oddOrEven = startOnLowerCase ? 0 : 1;
                    for (var i = 0; i < data.Length; i++)
                    {
                        if ((i % 2) == oddOrEven)
                            builder.Append(data[i].ToString().ToLowerInvariant());
                        else
                            builder.Append(data[i].ToString().ToUpperInvariant());
                    }

                    context.Result = builder.ToString();
                }

                return Task.CompletedTask;
            };
        }
    }
}