namespace GraphQL.AspNet.Examples.CustomDirectives.Directives
{
    using GraphQL.AspNet.Attributes;
    using GraphQL.AspNet.Directives;
    using GraphQL.AspNet.Interfaces.Controllers;
    using GraphQL.AspNet.Schemas.TypeSystem;
    using System;
    using System.Text;

    /// <summary>
    /// An execution directive that can be applied to any field in query document.
    /// When the resolved value of the field is a string convert that string to be
    /// in "SaRcAsTiC CaSiNg"
    /// </summary>
    public class ToSarcasticDirective : GraphDirective
    {
        /// <summary>
        /// This execution directive can only be applied directly to a field
        /// in a query document.
        /// </summary>
        [DirectiveLocations(DirectiveLocation.FIELD)]
        public IGraphActionResult Execute()
        {
            // for directives executing during query execution
            // DirectiveTarget will be the resolved field value
            //
            // This resolved field value may be a scalar or enum for leaf fields
            // or an object/scalar refrence for intermediate fields
            //
            if (this.DirectiveTarget != null && this.DirectiveTarget is string)
            {
                var builder = new StringBuilder();
                var data = this.DirectiveTarget.ToString();

                // randomly choose to start with lower case or upper case
                var oddOrEven = DateTime.UtcNow.Second % 2 == 0 ? 0 : 1;

                for (var i = 0; i < data.Length; i++)
                {
                    if ((i % 2) == oddOrEven)
                        builder.Append(data[i].ToString().ToLowerInvariant());
                    else
                        builder.Append(data[i].ToString().ToUpperInvariant());
                }

                // once the string is converted
                // replace the resolved value with the new string
                this.DirectiveTarget = builder.ToString();
            }

            // indicate that this directive completed
            // successfully
            return this.Ok();
        }
    }
}