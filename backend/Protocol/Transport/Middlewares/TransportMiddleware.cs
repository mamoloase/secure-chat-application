using Microsoft.AspNetCore.Http;

namespace Protocol.Transport.Middlewares;
public class TransportMiddleware
{
    private readonly RequestDelegate _next;
    private Connection _connection { get; set; }

    public TransportMiddleware(RequestDelegate next, Connection connection)
    {
        _next = next;
        _connection = connection;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.WebSockets.IsWebSocketRequest)
            return;

        var socket = await context.WebSockets.AcceptWebSocketAsync();

        var client = new ConnectionClient { Socket = socket, Context = context };

        await _connection.Handler(client: client);
    }

}
