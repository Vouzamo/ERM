﻿using System.Collections.Generic;
using Vouzamo.ERM.Common.Extensions;

namespace Vouzamo.ERM.Common.Models
{
    public class Editor
    {
        public string Localization { get; }
        public IEnumerable<PropertyEditor> Editors { get; }

        public Editor(string localization, IEnumerable<PropertyEditor> editors)
        {
            Localization = localization;
            Editors = editors;
        }
    }

    public class PropertyEditor
    {
        protected LocalizedValue LocalizedValue { get; }

        public Field Field { get; }
        public string Localization { get; }
        
        public object Fallback { get; set; }

        public bool ReadOnly => !Field.Localizable && !Localization.Equals(Constants.DefaultLocalization);
        public bool HasValue => LocalizedValue.ContainsKey(Localization);
        public object Value => ReadOnly ? LocalizedValue.ValueOrDefault(Constants.DefaultLocalization) : LocalizedValue.ValueOrDefault(Localization);

        public PropertyEditor(Field field, string localization, LocalizedValue localizedValue, object fallback = default)
        {
            Field = field;
            Localization = localization;
            LocalizedValue = localizedValue;
            Fallback = fallback;
        }
    }
}
