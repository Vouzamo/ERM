using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouzamo.ERM.Common;
using Vouzamo.ERM.Common.Extensions;
using Vouzamo.ERM.Common.Models;

namespace Vouzamo.ERM.CQRS.Extensions
{
    public static class FieldExtensions
    {
        public static async Task<IEnumerable<Hierarchy<Field>>> ExpandFields(this IEnumerable<Field> fields, IMediator mediator)
        {
            return await Task.WhenAll(fields.Select(field => field.ExpandField(mediator)));
        }

        public static async Task<Hierarchy<Field>> ExpandField(this Field field, IMediator mediator)
        {
            switch (field)
            {
                case NestedField nestedField:
                    var type = await mediator.Send(new ByIdQuery<Common.Type>(nestedField.TypeId));

                    var nestedFields = await type.Fields.ExpandFields(mediator);

                    return new Hierarchy<Field>(nestedField)
                    {
                        Children = nestedFields.ToList()
                    };
                default:
                    return new Hierarchy<Field>(field);
            }
        }

        public static Editor ToEditor(this IEnumerable<Hierarchy<Field>> fields, IDictionary<string, LocalizedValue> properties, IEnumerable<string> localizationChain)
        {
            var editors = fields.ToEditors(properties, localizationChain);

            return new Editor(localizationChain, editors);
        }

        public static IEnumerable<IEditor> ToEditors(this IEnumerable<Hierarchy<Field>> fields, IDictionary<string, LocalizedValue> properties, IEnumerable<string> localizationChain)
        {
            foreach(var field in fields)
            {
                var localizedValue = properties.ValueOrDefault(field.Source.Key, new LocalizedValue());

                yield return field.ToEditor(localizedValue, localizationChain);
            }
        }

        public static IEditor ToEditor(this Hierarchy<Field> field, LocalizedValue localizedValue, IEnumerable<string> localizationChain)
        {
            if (field.Children.Any())
            {
                var properties = new Dictionary<string, LocalizedValue>();
                
                if(localizedValue.TryGetValue(Constants.DefaultLocalization, out var value))
                {
                    properties = value as Dictionary<string, LocalizedValue>;
                }

                return new ObjectEditor(field.Source, field.Children.ToEditors(properties, localizationChain));
            }
            else
            {
                return new LiteralEditor(field.Source, localizedValue, localizationChain);
            }
        }
    }
}
