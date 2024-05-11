using System.ComponentModel.DataAnnotations;

namespace Protocol.Transport;
public class ConnectionHandlerParameter
{
    public bool IsValidRequest()
    {
        return Validator.TryValidateObject(
            instance: this,
            validationContext: new ValidationContext(this),
            validationResults: new List<ValidationResult>(), validateAllProperties: true);
    }
    public List<ValidationResult> GetValidationResults()
    {
        var validationResults = new List<ValidationResult>();

        Validator.TryValidateObject(
            instance: this,
            validationContext: new ValidationContext(this),
            validationResults: validationResults, validateAllProperties: true);

        return validationResults;
    }
}
