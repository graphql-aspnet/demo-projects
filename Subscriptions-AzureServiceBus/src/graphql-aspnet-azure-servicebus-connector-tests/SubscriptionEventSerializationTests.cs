namespace GraphQL.AspNet.Tests.AzureServiceBus
{
    using System;
    using System.Text.Json;
    using GraphQL.AspNet.AzureServiceBus.Serializers;
    using GraphQL.AspNet.Schemas;
    using GraphQL.AspNet.SubscriptionServer;
    using NUnit.Framework;

    [TestFixture]
    public class SubscriptionEventSerializationTests
    {
        public class TestWidget
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
        [Test]
        public void TestSubscriptionEventSerializationCycle()
        {
            // fake an event from graphql aspnet
            // that would be recieved by the publisher
            var widget = new TestWidget()
            {
                Id = 5,
                Name = "Widget5Name",
                Description = "Widget5Description"
            };

            var evt = new SubscriptionEvent()
            {
                Id = Guid.NewGuid().ToString(),
                EventName = "testEvent",
                DataTypeName = typeof(TestWidget).AssemblyQualifiedName,
                Data = widget,
                SchemaTypeName = typeof(GraphSchema).AssemblyQualifiedName,
            };

            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.WriteIndented = true;
            options.Converters.Add(new SubscriptionEventConverter());

            // serialize then deserialize the object
            var serailizedData = JsonSerializer.Serialize(evt, typeof(SubscriptionEvent), options);
            var deserialized = JsonSerializer.Deserialize<SubscriptionEvent>(serailizedData, options);

            // test all properties were translated correctly
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(evt.Id, deserialized.Id);
            Assert.AreEqual(evt.EventName, deserialized.EventName);
            Assert.AreEqual(evt.SchemaTypeName, deserialized.SchemaTypeName);
            Assert.AreEqual(evt.DataTypeName, deserialized.DataTypeName);

            var converted = deserialized.Data as TestWidget;
            Assert.IsNotNull(converted);
            Assert.AreEqual(converted.Id, widget.Id);
            Assert.AreEqual(converted.Name, widget.Name);
            Assert.AreEqual(converted.Description, widget.Description);
        }
    }
}