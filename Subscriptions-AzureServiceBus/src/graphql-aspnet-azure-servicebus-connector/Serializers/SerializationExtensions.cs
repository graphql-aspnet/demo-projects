namespace GraphQL.AspNet.AzureServiceBus.Serializers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Json;
    using GraphQL.AspNet.Common;

    /// <summary>
    /// Helper methods for deserializing objects
    /// </summary>
    internal static class SerializationExtensions
    {
        /// <summary>
        /// A delegate for a callback action to take when a property is
        /// read from a json reader.
        /// </summary>
        /// <param name="propName">Name of the property that was found.</param>
        /// <param name="reader">The reader.</param>
        /// <returns><c>true</c> if more properties should be read, <c>false</c> to immediately
        /// halt reading.</returns>
        public delegate bool Utf8JsonPropertyReaderCallBack(string propName, ref Utf8JsonReader reader);

        /// <summary>
        /// If the reader is pointing at a property name, will
        /// read through and execute the callback for each found
        /// property up until a property name is no longer found
        /// or the callback cancels the action.
        /// </summary>
        /// <param name="reader">The reader to walk.</param>
        /// <param name="readerAction">The reader action to take.
        /// return <c>false</c> to stop processing.</param>
        public static void ReadThroughProperties(
            this ref Utf8JsonReader reader,
            Utf8JsonPropertyReaderCallBack readerAction)
        {
            Validation.ThrowIfNull(readerAction, nameof(readerAction));

            var shouldContinue = true;
            while (shouldContinue
                && reader.TokenType == JsonTokenType.PropertyName)
            {
                // extract prop name
                var propName = reader.GetString();
                reader.Read();

                // mark where the reader is
                var bytesRead = reader.BytesConsumed;
                shouldContinue = readerAction(propName, ref reader);

                // when the callback didnt advance the reader
                // read through the currently pointed at
                // "property value"
                if (reader.BytesConsumed == bytesRead)
                    reader.Read();
            }
        }

        /// <summary>
        /// If the reader is pointing at the start of an object, this
        /// method will read through the object marker and execute
        /// the callback for each found property up until the end of the object
        /// or the callback cancels the action. This method will
        /// NOT consume the closing EndObject token if its present.
        /// </summary>
        /// <param name="reader">The reader to walk.</param>
        /// <param name="readerAction">The reader action to take.
        /// return <c>false</c> to stop processing.</param>
        public static void ReadThroughObjectProperties(
            this ref Utf8JsonReader reader,
            Utf8JsonPropertyReaderCallBack readerAction)
        {
            Validation.ThrowIfNull(readerAction, nameof(readerAction));

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                reader.Read();

                SerializationExtensions.ReadThroughProperties(
                    ref reader,
                    readerAction);
            }
        }

        /// <summary>
        /// Inspects the reader and if its pointing at a close array
        /// or close object, it reads through that token to the next one.
        /// </summary>
        /// <param name="reader">The reader to inspect.</param>
        public static void CloseOpenStructures(this ref Utf8JsonReader reader)
        {
            if (reader.TokenType == JsonTokenType.EndArray ||
                reader.TokenType == JsonTokenType.EndObject)
                reader.Read();
        }
    }
}
