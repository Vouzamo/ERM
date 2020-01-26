using System.Collections.Generic;

namespace Vouzamo.ERM.Common.Models.Validation
{
    public class ValueValidationResult : IValidationResult
    {
        public bool Valid { get; set; }
        public List<IValidationMessage> Messages { get; }

        public ValueValidationResult(bool valid)
        {
            Valid = valid;
            Messages = new List<IValidationMessage>();
        }

        public ValueValidationResult(bool valid, List<IValidationMessage> messages) : this(valid)
        {
            Messages = messages;
        }
    }
}
