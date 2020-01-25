using Vouzamo.ERM.Common.Extensions;

namespace Vouzamo.ERM.Common.Models
{
    public class PropertyEditor
    {
        public Field Field { get; }
        public string Localization { get; }
        public LocalizedValue LocalizedValue { get; set; }
        public object Fallback { get; set; }

        public bool ReadOnly => !Field.Localizable && !Localization.Equals("default");
        public bool HasValue => LocalizedValue.ContainsKey(Localization);
        public object Value => ReadOnly ? LocalizedValue.ValueOrDefault("default") : LocalizedValue.ValueOrDefault(Localization);

        public PropertyEditor(Field field, string localization = "default", LocalizedValue localizedValue = default, object fallback = default)
        {
            Field = field;
            Localization = localization;
            LocalizedValue = localizedValue ?? new LocalizedValue();
            Fallback = fallback;
        }
    }
}
