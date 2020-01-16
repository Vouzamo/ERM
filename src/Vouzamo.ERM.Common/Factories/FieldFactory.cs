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
                    return new StringField(field.Key, field.Name, field.Mandatory, field.Enumerable);
                default:
                    throw new ArgumentException("Field must be of a known type", nameof(field.Type));
            }
        }
    }
}
