namespace GraphQL.AspNet.Examples.LoggingProvider.Provider
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using GraphQL.AspNet.Common;
    using GraphQL.AspNet.Interfaces.Logging;

    /// <summary>
    /// A factory for generating a converter of either a master log entry one of its
    /// properties (which is also may be a property collection).
    /// </summary>
    public class LogEntryConverterFactory : JsonConverterFactory
    {
        /// <summary>
        /// When overridden in a derived class, determines whether the converter instance can convert the specified object type.
        /// </summary>
        /// <param name="typeToConvert">The type of the object to check whether it can be converted by this converter instance.</param>
        /// <returns><see langword="true" /> if the instance can convert the specified object type; otherwise, <see langword="false" />.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            return Validation.IsCastable<IGraphLogEntryPropertyCollection>(typeToConvert);
        }

        /// <summary>
        /// Creates a converter for a specified type.
        /// </summary>
        /// <param name="typeToConvert">The type handled by the converter.</param>
        /// <param name="options">The serialization options to use.</param>
        /// <returns>A converter for which <typeparamref name="T" /> is compatible with <paramref name="typeToConvert" />.</returns>
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new JsonLogPropertyCollectionConverter();
        }
    }
}