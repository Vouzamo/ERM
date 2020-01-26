using System;

namespace Vouzamo.ERM.Common.Models.Validation
{
    public class ExceptionValidationResult : ValueValidationResult
    {
        public Exception Exception { get; }

        public ExceptionValidationResult(Exception exception) : base(false)
        {
            Exception = exception;
        }
    }
}
