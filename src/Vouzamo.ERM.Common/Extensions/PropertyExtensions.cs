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
        public static IDictionary<string, object> AsValues(this Editor editor)
        {
            var values = new Dictionary<string, object>();

            foreach(var propertyEditor in editor.Editors)
            {
                var value = propertyEditor.Value;

                if(!propertyEditor.HasValue)
                {
                    if(propertyEditor.Field.Localizable)
                    {
                        value = propertyEditor.Fallback;
                    }
                }

                values.Add(propertyEditor.Field.Key, value);
            }

            return values;
        }

        public static Editor AsEditor(this IEnumerable<Field> fields, IDictionary<string, LocalizedValue> properties, IEnumerable<string> localizationChain)
        {
            var editors = new List<PropertyEditor>();

            var localization = localizationChain.First();

            var fallbacks = localizationChain.Count() > 1 ? Localize(properties, localizationChain.Skip(1)) : new Dictionary<string, object>();

            foreach(var field in fields)
            {
                var localizedValue = properties.ValueOrDefault(field.Key, new LocalizedValue());

                var editor = new PropertyEditor(field, localization, localizedValue);

                if(field.Localizable && fallbacks.TryGetValue(field.Key, out var fallback))
                {
                    editor.Fallback = fallback;
                }
                
                editors.Add(editor);
            }

            return new Editor(localization, editors);
        }

        public static IDictionary<string, object> Localize(this IDictionary<string, LocalizedValue> properties, IEnumerable<string> localizationHierarchy = default)
        {
            if(localizationHierarchy == default || !localizationHierarchy.Any())
            {
                localizationHierarchy = new List<string> { Constants.DefaultLocalization };
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

        /// <summary>
        /// This extension method has too many dependencies. Consider refactor...
        /// </summary>
        /// <param name="editors"></param>
        /// <param name="source"></param>
        /// <param name="localization"></param>
        /// <param name="localizationChain"></param>
        /// <param name="converter"></param>
        /// <param name="props"></param>
        /// <returns></returns>
        public static IEnumerable<IValidationResult> ValidateProperties(this IEnumerable<PropertyEditor> editors, IHasProperties<LocalizedValue> source, string localization, IEnumerable<string> localizationChain, IConverter converter, IDictionary<string, object> props) 
        {
            foreach (var editor in editors)
            {
                var key = editor.Field.Key;

                if (!source.Properties.ContainsKey(key))
                {
                    source.Properties.Add(key, new LocalizedValue());
                }

                if (editor.ReadOnly || !props.ContainsKey(key))
                {
                    source.Properties[key].Remove(localization);
                }
                else
                {
                    source.Properties[key][localization] = props[key];
                }

                if (source.Properties[key].TryGetLocalizedValue(localizationChain, out var value))
                {
                    yield return editor.Field.Validate(value, converter);
                }
                else
                {
                    var mandatoryResult = new ValueValidationResult(!editor.Field.Mandatory);

                    if (!mandatoryResult.Valid)
                    {
                        mandatoryResult.Messages.Add(new PropertyErrorValidationMessage(editor.Field.Key, $"Mandatory fields must provide a default value"));
                    }

                    yield return mandatoryResult;
                }
            }
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
