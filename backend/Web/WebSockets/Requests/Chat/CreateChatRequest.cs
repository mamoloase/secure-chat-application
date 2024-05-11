using System.ComponentModel.DataAnnotations;
using Web.Common.Domain.Enums;

namespace Web.WebSockets.Requests.Chat;
public class CreateChatRequest
{
    [Required]
    public string Name { get; set; }
    public ChatType Type { get; set; }
}
