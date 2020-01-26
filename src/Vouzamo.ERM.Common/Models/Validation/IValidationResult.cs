using System.Collections.Generic;

namespace Vouzamo.ERM.Common.Models.Validation
{
    public interface IValidationResult
    {
        bool Valid { get; }
        List<IValidationMessage> Messages { get; }
    }
}
