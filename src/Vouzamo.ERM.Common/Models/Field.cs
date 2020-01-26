using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Vouzamo.ERM.Common.Converters;
using Vouzamo.ERM.Common.Models;
using Vouzamo.ERM.Common.Models.Validation;
using Vouzamo.ERM.Common.Serialization;

namespace Vouzamo.ERM.Common
{
    public abstract class Field : IEquatable<Field>
    {
        public abstract string Type { get; }

        public string Key { get; set; }
        public string Name { get; set; }
        public bool Mandatory { get; set; }
        public bool Enumerable { get; set; }
        public bool Localizable { get; set; }
        

        protected Field()
        {
            Mandatory = false;
            Enumerable = false;
            Localizable = true;
        }

        public Field(string key, string name, bool mandatory = false, bool enumerable = false, bool localizable = false) : this()
        {
            Key = key;
            Name = name;
            Mandatory = mandatory;
            Enumerable = enumerable;
            Localizable = localizable;
        }

        public abstract IValidationResult Validate(object value, IConverter converter);

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
                   Localizable.Equals(other.Localizable) &&
                   Type.Equals(other.Type);

        }
    }

    public abstract class Field<T> : Field
    {
        public sealed override IValidationResult Validate(object value, IConverter converter)
        {
            IValidationResult result;

            try
            {
                if (Enumerable)
                {
                    var typedValues = converter.Convert<object, List<T>>(value);

                    result = new AggregateValidationResult(typedValues.Select(typedValue => ValidateValue(typedValue)));
                }
                else
                {
                    var typedValue = converter.Convert<object, T>(value);

                    result = ValidateValue(typedValue);
                }
            }
            catch (Exception ex)
            {
                result = new ExceptionValidationResult(ex);

                if (!result.Valid)
                {
                    result.Messages.Add(new PropertyErrorValidationMessage(Key, $"Couldn't coerce the value into the expected type: {Type}"));
                }
            }

            return result;
        }

        public virtual IValidationResult ValidateValue(T value)
        {
            var valid = !EqualityComparer<T>.Default.Equals(value, default) || !Mandatory;

            var result = new ValueValidationResult(valid);

            if (!result.Valid)
            {
                result.Messages.Add(new PropertyErrorValidationMessage(Key, $"Mandatory fields must specify or inherit a value"));
            }

            return result;
        }

        protected Field() : base()
        {

        }

        public Field(string key, string name, bool mandatory = false, bool enumerable = false, bool localizable = true) : base(key, name, mandatory, enumerable, localizable)
        {

        }
    }
}
