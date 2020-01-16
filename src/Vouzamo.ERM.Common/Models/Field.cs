using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Vouzamo.ERM.Common
{
    public abstract class Field : IEquatable<Field>
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public bool Mandatory { get; set; }
        public bool Enumerable { get; set; }
        public abstract string Type { get; }

        protected Field()
        {
            Mandatory = false;
            Enumerable = false;
        }

        public Field(string key, string name, bool mandatory = false, bool enumerable = false) : this()
        {
            Key = key;
            Name = name;
            Mandatory = mandatory;
            Enumerable = enumerable;
        }

        public abstract bool TryBuildValue(object raw, out object value);

        public override int GetHashCode()
        {
            return 990326508 + Key.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Field);
        }

        public bool Equals(Field other)
        {
            return other != null &&
                   Key.Equals(other.Key) &&
                   Name.Equals(other.Name) &&
                   Mandatory.Equals(other.Mandatory) &&
                   Enumerable.Equals(other.Enumerable) &&
                   Type.Equals(other.Type);

        }
    }

    public abstract class Field<T> : Field
    {
        public override bool TryBuildValue(object raw, out object value)
        {
            var json = JsonSerializer.Serialize(raw);

            if (Enumerable)
            {
                value = JsonSerializer.Deserialize<List<T>>(json);
            }
            else
            {
                value = JsonSerializer.Deserialize<T>(json);
            }

            return value != default;
        }

        protected Field() : base()
        {

        }

        public Field(string key, string name, bool mandatory = false, bool enumerable = false) : base(key, name, mandatory, enumerable)
        {

        }
    }
}
