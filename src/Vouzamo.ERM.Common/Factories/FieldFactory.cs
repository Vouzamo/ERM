using System;
using Vouzamo.ERM.DTOs;

namespace Vouzamo.ERM.Common.Factories
{
    public static class FieldFactory
    {
        public static Field CreateField(FieldDTO field)
        {
            switch (field.Type)
            {
                case "string":
                    return new StringField(field.Key, field.Name, field.Mandatory, field.Enumerable)
                    {
                        MinLength = field.MinLength,
                        MaxLength = field.MaxLength
                    };
                case "int":
                    return new IntegerField(field.Key, field.Name, field.Mandatory, field.Enumerable)
                    {
                        MinValue = field.MinValue,
                        MaxValue = field.MaxValue
                    };
                default:
                    throw new ArgumentException("Field must be of a known type", nameof(field.Type));
            }
        }
    }
}
