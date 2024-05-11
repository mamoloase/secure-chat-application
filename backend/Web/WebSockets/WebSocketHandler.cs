using Web.WebSockets.Handlers;

using Protocol.Transport;

namespace Web.WebSockets;

public class WebSocketHandler : Connection
{
    public WebSocketHandler(IServiceScopeFactory scopeFactory)
        : base(scopeFactory)
    {
        Handlers.Add(typeof(ChatHandler));
        Handlers.Add(typeof(UserHandler));
        Handlers.Add(typeof(MessageHandler));
        Handlers.Add(typeof(AuthenticationHandler));
    }

}
