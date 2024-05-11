
using Web.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Web.Common.Attributes;
public class PhoneNumberAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
        => ValidationExtensions.IsValidPhoneNumber((string)value);
        
    public override string FormatErrorMessage(string name)
    {
        return "INVALID_PHONE_NUMBER";
    }
}
