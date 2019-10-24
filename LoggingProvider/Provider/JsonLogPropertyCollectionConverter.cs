namespace GraphQL.AspNet.Examples.LoggingProvider.Provider
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using GraphQL.AspNet.Interfaces.Logging;

    /// <summary>
    /// A converter for serializing a log entry's property collection. This converter does not
    /// read data.
    /// </summary>
    public class JsonLogPropertyCollectionConverter : JsonConverter<IGraphLogPropertyCollection>
    {
        /// <summary>
        /// Reads and converts the JSON to type <typeparamref name="T" />.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public override IGraphLogPropertyCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException("This converter does not support reading Json data");
        }

        /// <summary>
        /// Writes a specified value as JSON.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="logEntry">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, IGraphLogPropertyCollection logEntry, JsonSerializerOptions options)
        {
            if (logEntry == null)
                return;

            writer.WriteStartObject();
            foreach (var kvp in logEntry)
            {
                writer.WritePropertyName(kvp.Key);

                if (kvp.Value == null)
                {
                    writer.WriteNullValue();
                }
                else
                {
                    JsonSerializer.Serialize(writer, kvp.Value, options);
                }
            }

            writer.WriteEndObject();
        }
    }
}