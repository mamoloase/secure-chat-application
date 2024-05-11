using System.Text;
using System.Net.WebSockets;

using Microsoft.AspNetCore.Http;

using Protocol.Serializer;
using Protocol.Transport.Managers;

namespace Protocol.Transport;
public class ConnectionClient
{
    public WebSocket Socket { get; set; }
    public HttpContext Context { get; set; }

    public CryptographyManager Cryptography { get; set; } = new();
    public AuthenticationManager Authentication { get; set; } = new();

    public virtual async Task SendAsync(
        ConnectionMessage message, ConnectionMessageType messageType = ConnectionMessageType.Encrypt, CancellationToken cancellationToken = default)
    {
        if (Socket.State != WebSocketState.Open) return;

        var payload = message.Serialize();

        if (messageType == ConnectionMessageType.Encrypt)
            payload = await Cryptography.EncryptMessage(message);

        var buffer = Encoding.UTF8.GetBytes(payload);

        await Socket.SendAsync(
            buffer: buffer,
            messageType: WebSocketMessageType.Binary,
            endOfMessage: true,
            cancellationToken: cancellationToken
        );
    }
}
