using Vouzamo.ERM.Common.Models;
using Vouzamo.ERM.Common.Models.Validation;

namespace Vouzamo.ERM.Common
{
    public class IntegerField : Field<int>
    {
        public override string Type => "int";

        public int MinValue { get; set; } = int.MinValue;
        public int MaxValue { get; set; } = int.MaxValue;

        protected IntegerField() : base()
        {

        }

        public IntegerField(string key, string name, bool mandatory = false, bool enumerable = false, bool localizable = true) : base(key, name, mandatory, enumerable, localizable)
        {

        }

        public override IValidationResult ValidateValue(int value)
        {
            var result = base.ValidateValue(value);

            if(value != default)
            {
                if (value < MinValue)
                {
                    result.Messages.Add(new PropertyErrorValidationMessage(Key, $"Value must be no less than {MinValue}"));

                    result = new ValueValidationResult(false, result.Messages);
                }

                if (value > MaxValue)
                {
                    result.Messages.Add(new PropertyErrorValidationMessage(Key, $"Value must be no greater than {MaxValue}"));

                    result = new ValueValidationResult(false, result.Messages);
                }
            }

            return result;
        }
    }
}
