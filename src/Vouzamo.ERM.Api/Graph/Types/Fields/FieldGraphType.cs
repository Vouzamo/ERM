﻿using GraphQL.Types;
using Vouzamo.ERM.Common;

namespace Vouzamo.ERM.Api.Graph.Types
{
    public abstract class FieldGraphType<T> : ObjectGraphType<T> where T : Field
    {
        public FieldGraphType()
        {
            Field(field => field.Type);
            Field(field => field.Key);
            Field(field => field.Name);
            Field(field => field.Mandatory);
            Field(field => field.Enumerable);
            Field(field => field.Localizable);

            Interface<FieldInterface>();
        }
    }
}
