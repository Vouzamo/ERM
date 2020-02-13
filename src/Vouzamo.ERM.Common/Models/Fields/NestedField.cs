using System;
using System.Collections.Generic;
using Vouzamo.ERM.Common.Extensions;
using Vouzamo.ERM.Common.Models.Validation;

namespace Vouzamo.ERM.Common
{
    public class NestedField : Field<IDictionary<string, LocalizedValue>>
    {
        public override string Type => "nested";

        public Guid TypeId { get; set; }

        protected NestedField() : base()
        {

        }

        public NestedField(string key, string name) : base(key, name)
        {
            
        }
    }
}
