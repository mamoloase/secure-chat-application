using Protocol.Transport;
using Web.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Web.WebSockets.Requests.Authentication;
public class VerificationCodeRequest : ConnectionHandlerParameter
{
    [Required]
    [PhoneNumber]
    public string Phone { get; set; }

    [Required]
    public string Code { get; set; }
}
