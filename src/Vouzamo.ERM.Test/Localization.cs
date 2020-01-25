using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Extensions;

namespace Vouzamo.ERM.Test
{
    [TestClass]
    public class Localization
    {
        protected List<Field> Fields { get; }
        protected Dictionary<string, LocalizedValue> Properties { get; }
        protected IEnumerable<string> LocalizationHierarchy { get; }

        public Localization()
        {
            Fields = new List<Field>
            {
                new StringField("givenName", "Given Name", true, false),
                new StringField("middleNames", "Middle Names", false, true),
                new StringField("familyName", "Family Name", true, false),
                new IntegerField("age", "Age")
            };

            Properties = new Dictionary<string, LocalizedValue>
            {
                { "givenName", new LocalizedValue
                    {
                        { "default", "John" },
                        { "fr", "John (French)" }
                    }
                },
                { "middleNames", new LocalizedValue
                    {
                        { "fr", new List<string> { "Raymond", "French" } }
                    }
                },
                { "familyName", new LocalizedValue
                    {
                        { "default", "Askew" },
                        { "fr-CA", "Askew (French Canadian)" }
                    }
                },
                { "age", new LocalizedValue
                    {
                        { "default", 35 },
                        { "fr-CA", null }
                    }
                }
            };

            LocalizationHierarchy = new List<string>
            {
                "fr-CA",
                "fr",
                "default"
            };
        }

        [TestMethod]
        public void NonMandatoryLocalizedValue()
        {
            var frCA = Properties.Localize(LocalizationHierarchy);
            var fr = Properties.Localize(LocalizationHierarchy.Skip(1));

            Assert.AreEqual(35, fr["age"]);
            Assert.IsFalse(frCA.ContainsKey("age"));
        }

        [TestMethod]
        public void MandatoryLocalizedValue()
        {
            var frCA = Properties.Localize(LocalizationHierarchy);
            var en = Properties.Localize();

            Assert.AreEqual("Askew", en["familyName"]);
            Assert.AreEqual("Askew (French Canadian)", frCA["familyName"]);
        }

        [TestMethod]
        public void PropertyEditors()
        {
            var editors = Fields.AsEditors(Properties, LocalizationHierarchy);

            var familyName = editors.Single(editor => editor.Field.Key.Equals("familyName"));

            Assert.AreEqual(4, editors.Count());
            Assert.AreEqual("Askew (French Canadian)", familyName.Value);
            Assert.AreEqual("Askew", familyName.Fallback);
        }
    }
}
