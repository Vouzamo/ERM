﻿using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.Api.Graph.Types.Fields
{
    public class IntegerFieldGraphType : FieldGraphType<IntegerField>
    {
        public IntegerFieldGraphType()
        {
            Name = "int";

            Field(field => field.MinValue);
            Field(field => field.MaxValue);
        }
    }
}
