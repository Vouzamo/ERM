using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.Json;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Serialization;

namespace Vouzamo.ERM.Test
{
    [TestClass]
    public class Serialization
    {
        protected JsonSerializerOptions Options { get; } = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public Serialization()
        {
            Options.Converters.Add(new FieldConverter());
            Options.Converters.Add(new ObjectToPrimitiveConverter());
        }

        [TestMethod]
        public void NodeType()
        {
            var original = new NodeType(Guid.NewGuid(), "Car");
            original.Fields.Add(new StringField("manufacturer", "Manufacturer"));

            var json = JsonSerializer.Serialize(original, Options);
            var deserialized = JsonSerializer.Deserialize<NodeType>(json, Options);

            Assert.AreEqual(original, deserialized);
        }
    }
}
