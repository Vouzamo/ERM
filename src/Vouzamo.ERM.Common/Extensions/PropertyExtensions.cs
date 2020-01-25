using System;
using System.Collections.Generic;
using System.Linq;
using Vouzamo.ERM.Common.Models;

namespace Vouzamo.ERM.Common.Extensions
{
    public static class PropertyExtensions
    {
        public static IDictionary<string, object> AsValues(this IEnumerable<PropertyEditor> editors)
        {
            var values = new Dictionary<string, object>();

            foreach(var editor in editors)
            {
                var value = editor.Value;

                if(!editor.HasValue)
                {
                    if(editor.Field.Localizable)
                    {
                        value = editor.Fallback;
                    }
                }

                values.Add(editor.Field.Key, value);
            }

            return values;
        }

        public static IEnumerable<PropertyEditor> AsEditors(this IEnumerable<Field> fields, IDictionary<string, LocalizedValue> properties, IEnumerable<string> localizationChain)
        {
            var localization = localizationChain.First();

            var fallbacks = localizationChain.Count() > 1 ? Localize(properties, localizationChain.Skip(1)) : new Dictionary<string, object>();

            foreach(var field in fields)
            {
                var editor = new PropertyEditor(field, localization);

                if (properties.TryGetValue(field.Key, out var localizedValue))
                {
                    editor.LocalizedValue = localizedValue;
                }

                if(field.Localizable && fallbacks.TryGetValue(field.Key, out var fallback))
                {
                    editor.Fallback = fallback;
                }
                
                yield return editor;
            }
        }

        public static IDictionary<string, object> Localize(this IDictionary<string, LocalizedValue> properties, IEnumerable<string> localizationHierarchy = default)
        {
            if(localizationHierarchy == default || !localizationHierarchy.Any())
            {
                localizationHierarchy = new List<string> { "default" };
            }

            var localized = new Dictionary<string, object>();

            foreach(var kvp in properties)
            {
                if(properties.TryGetValue(kvp.Key, out var localizedProperty))
                {
                    if(localizedProperty.TryGetLocalizedValue(localizationHierarchy, out var value))
                    {
                        localized.Add(kvp.Key, value);
                    }
                }
            }

            return localized;
        }

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
