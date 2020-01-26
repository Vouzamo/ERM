namespace Vouzamo.ERM.Common.Models.Validation
{
    /// <summary>
    /// Needs a JsonConverter to handle deserialization
    /// </summary>
    public class PropertyErrorValidationMessage : IValidationMessage
    {
        public string Type => "property";

        public string Reference { get; }
        public string Message { get; }

        public PropertyErrorValidationMessage(string reference, string message)
        {
            Reference = reference;
            Message = message;
        }
    }
}
