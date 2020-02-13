using System;
using System.Collections.Generic;
using System.Linq;
using Vouzamo.ERM.Common.Extensions;

namespace Vouzamo.ERM.Common.Models
{
    public class Editor
    {
        public IEnumerable<string> LocalizationChain { get; }
        public IEnumerable<IEditor> Editors { get; }

        public Editor(IEnumerable<string> localizationChain, IEnumerable<IEditor> editors)
        {
            LocalizationChain = localizationChain;
            Editors = editors;
        }

        public object BuildObject()
        {
            var dictionary = new Dictionary<string, object>();

            foreach(var editor in Editors.Where(editor => editor.ValueOrFallback != default))
            {
                dictionary.Add(editor.Key, editor.ValueOrFallback);
            }

            return dictionary;
        }
    }

    public interface IEditor
    {
        string Type { get; }
        string Key { get; }
        string Name { get; }
        bool Mandatory { get; }
        bool Enumerable { get; }
        object Value { get; }
        object FallbackValue { get; }
        object ValueOrFallback { get; }
    }

    public abstract class BaseEditor : IEditor
    {
        public abstract string Type { get; }
        public string Key => Field.Key;
        public string Name => Field.Name;
        public bool Mandatory => Field.Mandatory;
        public bool Enumerable => Field.Enumerable;
        public abstract object Value { get; }
        public abstract object FallbackValue { get; }
        public abstract object ValueOrFallback { get; }

        protected Field Field { get; }

        public BaseEditor(Field field)
        {
            Field = field;
        }
    }

    public class LiteralEditor : BaseEditor
    {
        public override string Type => "literal";
        public override object Value => LocalizedValue.ValueOrDefault(LocalizationChain.First());
        public override object FallbackValue => LocalizedValue.TryGetLocalizedValue(LocalizationChain.Count() > 1 ? LocalizationChain.Skip(1) : LocalizationChain, out var value) ? value : default;
        public override object ValueOrFallback => Value ?? FallbackValue;

        private IEnumerable<string> LocalizationChain { get; }
        private LocalizedValue LocalizedValue { get; }

        public LiteralEditor(Field field, LocalizedValue localizedValue, IEnumerable<string> localizationChain) : base(field)
        {
            LocalizedValue = localizedValue;
            LocalizationChain = localizationChain;
        }
    }

    public class ObjectEditor : BaseEditor
    {
        public override string Type => "object";
        public override object Value => BuildObject(e => e.Value);
        public override object FallbackValue => BuildObject(e => e.FallbackValue);
        public override object ValueOrFallback => BuildObject(e => e.ValueOrFallback);

        public IEnumerable<IEditor> Editors { get; }

        public ObjectEditor(Field field, IEnumerable<IEditor> editors) : base(field)
        {
            Editors = editors;
        }

        private object BuildValue()
        {
            var dictionary = new Dictionary<string, object>();

            foreach(var editor in Editors.Where(editor => editor.Value != default))
            {
                dictionary.Add(editor.Key, editor.Value);
            }

            return dictionary;
        }

        private object BuildFallbackValue()
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var editor in Editors.Where(editor => editor.FallbackValue != default))
            {
                dictionary.Add(editor.Key, editor.FallbackValue);
            }

            return dictionary;
        }

        private object BuildValueOrFallback()
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var editor in Editors.Where(editor => editor.ValueOrFallback != default))
            {
                dictionary.Add(editor.Key, editor.ValueOrFallback);
            }

            return dictionary;
        }

        private object BuildObject<TKey>(Func<IEditor, TKey> keySelector)
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var editor in Editors.Where(e => !keySelector.Invoke(e).Equals(default)))
            {
                dictionary.Add(editor.Key, keySelector.Invoke(editor));
            }

            return dictionary;
        }
    }
}
