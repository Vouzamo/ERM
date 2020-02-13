using System;
using System.Collections.Generic;
using System.Linq;
using Vouzamo.ERM.Common.Converters;
using Vouzamo.ERM.Common.Models;
using Vouzamo.ERM.Common.Models.Validation;

namespace Vouzamo.ERM.Common.Extensions
{
    public static class PropertyExtensions
    {
        //public static IEnumerable<IValidationResult> ValidateProperties(this IEnumerable<PropertyEditor> editors, IDictionary<string, LocalizedValue> source, string localization, IEnumerable<string> localizationChain, IConverter converter, IDictionary<string, object> props) 
        //{
        //    foreach (var editor in editors)
        //    {
        //        var key = editor.Field.Key;

        //        if (!source.ContainsKey(key))
        //        {
        //            source.Add(key, new LocalizedValue());
        //        }

        //        if (editor.ReadOnly || !props.ContainsKey(key))
        //        {
        //            source[key].Remove(localization);
        //        }
        //        else
        //        {
        //            source[key][localization] = props[key];
        //        }

        //        if (source[key].TryGetLocalizedValue(localizationChain, out var value))
        //        {
        //            yield return editor.Field.Validate(value, converter);
        //        }
        //        else
        //        {
        //            var mandatoryResult = new ValueValidationResult(!editor.Field.Mandatory);

        //            if (!mandatoryResult.Valid)
        //            {
        //                mandatoryResult.Messages.Add(new PropertyErrorValidationMessage(editor.Field.Key, $"Mandatory fields must provide a default value"));
        //            }

        //            yield return mandatoryResult;
        //        }
        //    }
        //}

        public static bool TryGetLocalizedValue(this LocalizedValue localized, IEnumerable<string> localizationHierarchy, out object value)
        {
            value = default;

            foreach (var localization in localizationHierarchy)
            {
                if (localized.ContainsKey(localization))
                {
                    value = localized[localization];

                    break;
                }
            }

            return value != default;
        }

        public static TValue ValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            if(dictionary != null && dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return defaultValue;
        }

        public static bool TryFindDependencyChain<T>(this Hierarchy<T> hierarchy, T source, out List<T> chain)
        {
            chain = new List<T>();

            if (hierarchy.Source.Equals(source))
            {
                chain.Add(hierarchy.Source);

                return true;
            }

            foreach (var prospect in hierarchy.Children)
            {
                if (prospect.TryFindDependencyChain(source, out chain))
                {
                    chain.Add(hierarchy.Source);

                    return true;
                }
            }

            return false;
        }

        public static List<T> FindDependencyChain<T>(this Hierarchy<T> hierarchy, T source)
        {
            if(!hierarchy.TryFindDependencyChain(source, out var chain))
            {
                chain = new List<T> { source };
            }

            return chain;
        }
    }
}
