using Protocol.Transport;
using System.ComponentModel.DataAnnotations;

namespace Web.WebSockets.Requests.Authentication;
public class AuthenticationRequest : ConnectionHandlerParameter
{
    [Required]
    public string Token { get; set; }
}
