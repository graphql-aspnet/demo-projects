namespace GraphQL.AspNet.AzureServiceBus.Serializers
{
    using GraphQL.AspNet.SubscriptionServer;
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// A serializer for converting a graphql-aspnet <see cref="SubscriptionEvent"/>
    /// into a message format (a json string) that can be placed on the Azure Service Bus
    /// </summary>
    public class SubscriptionEventConverter : JsonConverter<SubscriptionEvent>
    {
        /// <summary>
        /// Reads and converts the JSON to type <typeparamref name="SubscriptionEvent" />.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public override SubscriptionEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string id = null;
            object data = null;
            string dataTypeName = null;
            string eventName = null;
            string schemaTypeName = null;

            reader.ReadThroughObjectProperties(
               (string propName, ref Utf8JsonReader r) =>
            {
                switch (propName)
                {
                    case nameof(SubscriptionEvent.Id):
                        id = r.GetString();
                        return true;

                    case nameof(SubscriptionEvent.DataTypeName):
                        dataTypeName = r.GetString();
                        return true;

                    case nameof(SubscriptionEvent.SchemaTypeName):
                        schemaTypeName = r.GetString();
                        return true;

                    case nameof(SubscriptionEvent.EventName):
                        eventName = r.GetString();
                        return true;

                    case nameof(SubscriptionEvent.Data):
                        if (string.IsNullOrWhiteSpace(dataTypeName))
                        {
                            throw new JsonException(
                                $"Unable to deserialize the {nameof(SubscriptionEvent)}.{nameof(SubscriptionEvent.Data)} property " +
                                $"before the {nameof(SubscriptionEvent)}.{nameof(SubscriptionEvent.DataTypeName)} property.");
                        }

                        var dataType = Type.GetType(dataTypeName);
                        data = JsonSerializer.Deserialize(ref r, dataType, options);
                        r.CloseOpenStructures();
                        return true;

                    default:
                        throw new JsonException(
                            $"Invalid {nameof(SubscriptionEvent)}. The property name '{propName}' " +
                            "is unsupported for deserialization.");
                }
            });

            return new SubscriptionEvent()
            {
                Id = id,
                EventName = eventName,
                DataTypeName = dataTypeName,
                SchemaTypeName = schemaTypeName,
                Data = data
            };
        }

        /// <summary>
        /// Writes a specified subscription event as JSON data to the supplied writer.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, SubscriptionEvent value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(nameof(SubscriptionEvent.Id), value.Id);
            writer.WriteString(nameof(SubscriptionEvent.DataTypeName), value.DataTypeName);
            writer.WriteString(nameof(SubscriptionEvent.SchemaTypeName), value.SchemaTypeName);
            writer.WriteString(nameof(SubscriptionEvent.EventName), value.EventName);

            if (value.Data != null)
            {
                writer.WritePropertyName(nameof(SubscriptionEvent.Data));
                JsonSerializer.Serialize(writer, value.Data, value.Data.GetType(), options);
            }

            writer.WriteEndObject();
        }
    }
}