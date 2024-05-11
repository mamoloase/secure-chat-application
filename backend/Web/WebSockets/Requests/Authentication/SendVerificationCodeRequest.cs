using Protocol.Transport;
using Web.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Web.WebSockets.Requests.Authentication;
public class SendVerificationCodeRequest : ConnectionHandlerParameter
{
    [Required]
    [PhoneNumber]
    public string Phone { get; set; }

}
