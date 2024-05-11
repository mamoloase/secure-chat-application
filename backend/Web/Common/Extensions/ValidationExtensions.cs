using Web.Common.Constants;

using System.Text.RegularExpressions;

namespace Web.Common.Extensions;
public class ValidationExtensions
{
    public static bool IsValidPhoneNumber(string phone)
    {
        if (new Regex(RegexConstants.PhoneNumberRegex).IsMatch(phone) == false)
            return false;
            
        var countriesCode = PhoneExtensions.GetCountryCodeToRegionCodeMap();

        phone = PhoneExtensions.NormalizePhoneNumber(phone);

        if (countriesCode.Any(x => phone.StartsWith(x.Key.ToString())) == false)
            return false;

        return true;
    }
}
