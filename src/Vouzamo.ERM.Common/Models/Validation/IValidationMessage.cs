namespace Vouzamo.ERM.Common.Models.Validation
{
    public interface IValidationMessage
    {
        string Type { get; }
        string Reference { get; }
        string Message { get; }
    }
}
