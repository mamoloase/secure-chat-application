
using Protocol;
using Protocol.Extensions;

using Web;
using Web.Common;
using Web.WebSockets;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Services.AddScoped<UnitOfWork>();

builder.Services.AddConfigurations(configuration);

builder.Services.AddProtocolServices();

var app = builder.Build();

using IServiceScope scope = app.Services.CreateScope();

app.UseCors();

app.UseRouting();

app.UseWebSockets();

app.MapWebSocketManager("/socket",
    scope.ServiceProvider.GetService<WebSocketHandler>());

app.Run();
