using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Serialization;
using Vouzamo.ERM.Providers.Elasticsearch;

namespace Vouzamo.ERM.Test
{
    [TestClass]
    public class InMemoryProviderTests
    {
        protected IGraphProvider Provider { get; }
        protected string Locale = "en";

        public InMemoryProviderTests()
        {
            //Provider = new InMemoryGraphProvider();
            Provider = new ElasticsearchGraphProvider();
        }

        public async Task Seed()
        {
            var person = await Provider.CreateNodeType("Person");
            person.Fields.Add(new StringField("givenName", "Given Name", true));
            person.Fields.Add(new StringField("familyName", "Family Name", true));
            person.Fields.Add(new StringField("middleNames", "Middle Name(s)", false, true));
            var personUpdated = await Provider.UpdateNodeType(person);
            
            var animal = await Provider.CreateNodeType("Animal");
            
            var john = await Provider.CreateNode(person, "John Askew");
            john.Properties.Add("givenName", new LocalizedValue() {
                { Locale, "John" }
            });
            john.Properties.Add("middleNames", new LocalizedValue() {
                { Locale, new List<string>() { "Raymond" } }
            });
            john.Properties.Add("familyName", new LocalizedValue() {
                { Locale, "Askew" }
            });
            var johnUpdated = await Provider.UpdateNode(john);

            var charlotte = await Provider.CreateNode(person, "Charlotte Askew");
            charlotte.Properties.Add("givenName", new LocalizedValue() {
                { Locale, "Charlotte" }
            });
            charlotte.Properties.Add("middleNames", new LocalizedValue() {
                { Locale, new List<string>() { "Elizabeth" } }
            });
            charlotte.Properties.Add("familyName", new LocalizedValue() {
                { Locale, "Askew" }
            });
            var charlotteUpdated = await Provider.UpdateNode(charlotte);

            var marie = await Provider.CreateNode(person, "Marie Whatmough");
            marie.Properties.Add("givenName", new LocalizedValue() {
                { Locale, "Marie" }
            });
            marie.Properties.Add("familyName", new LocalizedValue() {
                { Locale, "Whatmough" }
            });
            var marieUpdated = await Provider.UpdateNode(marie);

            var buddy = await Provider.CreateNode(animal, "Buddy");
            var pom = await Provider.CreateNode(animal, "Pom");

            var isMarriedTo = await Provider.CreateEdgeType("Is Married To");
            var isChildOf = await Provider.CreateEdgeType("Is Child Of");
            var isMotherOf = await Provider.CreateEdgeType("Is Mother Of");
            var isPetOf = await Provider.CreateEdgeType("Is Pet Of");
            var livesWith = await Provider.CreateEdgeType("Lives With");

            await Provider.CreateEdge(john, isMarriedTo, charlotte);
            await Provider.CreateEdge(charlotte, isMarriedTo, john);
            await Provider.CreateEdge(charlotte, isChildOf, marie);
            await Provider.CreateEdge(marie, isMotherOf, charlotte);
            await Provider.CreateEdge(buddy, isPetOf, john);
            await Provider.CreateEdge(buddy, isPetOf, charlotte);
            await Provider.CreateEdge(pom, isPetOf, john);
            await Provider.CreateEdge(pom, isPetOf, charlotte);
            await Provider.CreateEdge(john, livesWith, charlotte);
            await Provider.CreateEdge(charlotte, livesWith, john);
        }

        [TestMethod]
        public async Task Traversal()
        {
            //await Seed();

            var nodes = await Provider.QueryNodes(string.Empty);

            var me = nodes.First(node => node.Properties["givenName"][Locale].Equals("John"));
            
            var myOutboundEdges = await Provider.EdgesFrom(me);
            var myInboundEdges = await Provider.EdgesTo(me);

            var pets = myInboundEdges.Where(edge => edge.Type.Name.Equals("Is Pet Of")).Select(edge => edge.Node);

            Assert.AreEqual(2, pets.Count());
            Assert.IsTrue(pets.Any(pet => pet.Name.Equals("Buddy")));
            Assert.IsTrue(pets.Any(pet => pet.Name.Equals("Pom")));

            var meAndCharlotte = myInboundEdges.Where(edge => edge.Node.Name.Equals("Charlotte Askew")).Select(edge => edge.Type.Name);

            Assert.AreEqual(2, meAndCharlotte.Count());
            Assert.IsTrue(meAndCharlotte.Any(us => us.Equals("Lives With")));

            var wife = myOutboundEdges.Single(edge => edge.Type.Name.Equals("Is Married To")).Node;

            Assert.AreEqual("Charlotte Askew", wife.Name);

            var wifesInboundEdges = await Provider.Traverse(wife, Direction.Inbound);

            var motherInLaw = wifesInboundEdges.Single(edge => edge.Type.Name.Equals("Is Mother Of")).Node;

            Assert.AreEqual("Marie Whatmough", motherInLaw.Name);
        }
    }
}
