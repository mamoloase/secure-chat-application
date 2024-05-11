using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Protocol.Transport;
using Protocol.Transport.Middlewares;

namespace Protocol.Extensions;
public static class EndpointExtensions
{
    public static IApplicationBuilder MapWebSocketManager(
        this IApplicationBuilder app, PathString path, Connection handler)
    {
        return app.Map(path, (_app) =>
            _app.UseMiddleware<TransportMiddleware>(handler));

    }
}