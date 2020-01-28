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
            var original = new Common.Type(Guid.NewGuid(), "Car", TypeScope.Nodes);
            original.Fields.Add(new StringField("manufacturer", "Manufacturer"));

            var json = JsonSerializer.Serialize(original, Options);
            var deserialized = JsonSerializer.Deserialize<Common.Type>(json, Options);

            Assert.AreEqual(original, deserialized);
        }
    }
}
