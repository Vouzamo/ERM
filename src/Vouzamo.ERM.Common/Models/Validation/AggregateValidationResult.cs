using System.Collections.Generic;
using System.Linq;

namespace Vouzamo.ERM.Common.Models.Validation
{
    public class AggregateValidationResult : IValidationResult
    {
        public bool Valid => Results.All(result => result.Valid);
        public List<IValidationMessage> Messages => Results.SelectMany(result => result.Messages).ToList();

        protected IEnumerable<IValidationResult> Results { get; }

        public AggregateValidationResult(IEnumerable<IValidationResult> results)
        {
            Results = results;
        }
    }
}
