using System;
using System.Collections.Generic;
using System.Linq;

namespace Vouzamo.ERM.Common
{
    public class Type : IHasName, IIdentifiable<Guid>, IHasFields, IEquatable<Type>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Field> Fields { get; set; }
        public TypeScope Scope { get; set; }

        protected Type()
        {
            Fields = new List<Field>();
        }

        public Type(Guid id, string name, TypeScope scope) : this()
        {
            Id = id;
            Name = name;
            Scope = scope;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Type);
        }

        public bool Equals(Type other)
        {
            return other != null &&
                   Id.Equals(other.Id) &&
                   Name == other.Name &&
                   Fields.SequenceEqual(other.Fields) &&
                   Scope == other.Scope;
        }

        public override int GetHashCode()
        {
            var hashCode = 447833837;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Field>>.Default.GetHashCode(Fields);
            hashCode = hashCode * -1521134295 + Scope.GetHashCode();
            return hashCode;
        }
    }
}
