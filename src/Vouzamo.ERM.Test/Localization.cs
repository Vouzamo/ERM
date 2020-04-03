using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Models;
using Vouzamo.ERM.CQRS.Extensions;

namespace Vouzamo.ERM.Test
{
    [TestClass]
    public class Localization
    {
        protected List<Hierarchy<Field>> Fields { get; }
        protected Dictionary<string, LocalizedValue> Properties { get; }
        protected IEnumerable<string> LocalizationHierarchy { get; }

        public Localization()
        {
            Fields = new List<Hierarchy<Field>>
            {
                new Hierarchy<Field>(new StringField("givenName", "Given Name") { Mandatory = true }),
                new Hierarchy<Field>(new StringField("middleNames", "Middle Names") { Enumerable = true }),
                new Hierarchy<Field>(new StringField("familyName", "Family Name") { Mandatory = true }),
                new Hierarchy<Field>(new IntegerField("age", "Age")),
                new Hierarchy<Field>(new StringField("interests", "Interests") { Enumerable = true }),
                new Hierarchy<Field>(new NestedField("address", "Address") { Mandatory = true })
                {
                    Children = new List<Hierarchy<Field>>
                    {
                        new Hierarchy<Field>(new StringField("street", "Street") { Mandatory = true }),
                        new Hierarchy<Field>(new StringField("city", "City") { Mandatory = true }),
                        new Hierarchy<Field>(new StringField("state", "State") { Mandatory = true }),
                    }
                }
            };

            Properties = new Dictionary<string, LocalizedValue>
            {
                { "givenName", new LocalizedValue
                    {
                        { Constants.DefaultLocalization, "John" },
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
                        { Constants.DefaultLocalization, "Askew" },
                        { "fr-CA", "Askew (French Canadian)" }
                    }
                },
                { "age", new LocalizedValue
                    {
                        { Constants.DefaultLocalization, 35 },
                        { "fr-CA", null }
                    }
                },
                {
                    "address", new LocalizedValue
                    {
                        { Constants.DefaultLocalization, new Dictionary<string, LocalizedValue>
                            {
                                { "street", new LocalizedValue
                                    {
                                        { Constants.DefaultLocalization, "3 Maxwell Dr" }
                                    }
                                },
                                { "city", new LocalizedValue
                                    {
                                        { Constants.DefaultLocalization, "Derry" }
                                    }
                                },
                                { "state", new LocalizedValue
                                    {
                                        { Constants.DefaultLocalization, "NH" }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            LocalizationHierarchy = new List<string>
            {
                "fr-CA",
                "fr",
                Constants.DefaultLocalization
            };
        }

        [TestMethod]
        public void EditorTest()
        {
            var editor = Fields.ToEditors(Properties, LocalizationHierarchy);
        }

        [TestMethod]
        public void PropertyEditors()
        {
            var props = new Dictionary<string, object>
            {
                { "givenName", "John" },
                { "middleNames", new List<string> { "Raymond" } },
                { "familyName", "Askew" },
                { "age", 35 },
                { "address", new Dictionary<string, object>
                    {
                        { "street", "3 Maxwell Dr" },
                        { "city", "Derry" },
                        { "state", "New Hampshire" }
                    }
                }
            };

            var editor = Fields.ToEditor(Properties, LocalizationHierarchy);

            var value = editor.BuildObject();

            var familyName = editor.Editors.Single(editor => editor.Key.Equals("familyName"));

            Assert.AreEqual(6, editor.Editors.Count());
            Assert.AreEqual("Askew (French Canadian)", familyName.Value);
            Assert.AreEqual("Askew", familyName.FallbackValue);
        }
    }
}
