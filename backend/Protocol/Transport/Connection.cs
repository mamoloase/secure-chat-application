using System.Text;
using System.Net.WebSockets;

using Microsoft.Extensions.DependencyInjection;

using Protocol.Serializer;
using Protocol.Extensions;
using Protocol.Transport.Managers;

namespace Protocol.Transport;
public class Connection
{
    public List<ConnectionClient> Clients { get; set; } = new();

    public readonly ConnectionGroupManager _groupManager = new();

    public List<Type> Handlers { get; set; } = new();

    private readonly IServiceScopeFactory _scopeFactory;

    public Connection(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    public virtual async Task Handler(ConnectionClient client, CancellationToken cancellationToken = default)
    {
        await OnConnectAsync(client);

        while (client.Socket.State == WebSocketState.Open)
        {
            var buffer = new byte[1024 * 1024];

            try
            {
                var request = await client.Socket.ReceiveAsync(
                    buffer: new ArraySegment<byte>(buffer), cancellationToken: cancellationToken);

                if (request.MessageType == WebSocketMessageType.Close)
                    break;

                var message = await client.Cryptography.DecryptMessage(
                    Encoding.UTF8.GetString(buffer, 0, request.Count));

                if (message != null) await OnReceiveRequestAsync(
                    client: client, message: message);
            }
            catch (Exception exception)
            {
                var response = new ConnectionMessage
                {
                    Handler = "Exception",
                    Method = "InternalServer",
                };
                response.Parameters.Add("message", exception.Message);

                await client.SendAsync(
                    message: response, messageType: ConnectionMessageType.Normall);
            }
        }
        await OnDisConnectAsync(client);
    }

    public virtual async Task OnConnectAsync(ConnectionClient client, CancellationToken cancellationToken = default)
    {
        var key = client.Context.Request.Query
                .SingleOrDefault(x => x.Key == "pk");

        if (client.Cryptography.TryLoadPublicKey(key.Value))
        {
            var response = new ConnectionMessage { Method = "initialize" };

            response.Parameters.Add("signiture",
                client.Cryptography.EncryptSigniture());

            await client.SendAsync(
                message: response,
                messageType: ConnectionMessageType.Normall,
                cancellationToken: cancellationToken);

            // Add connection to online connections 
            Clients.Add(client);

            return;
        }
        await OnDisConnectAsync(client);
    }

    public virtual async Task OnDisConnectAsync(ConnectionClient client, Exception exception = default, CancellationToken cancellationToken = default)
    {
        // After a disconnect request to the server, this method is called
        if (client.Socket.State == WebSocketState.Open)
            await client.Socket.CloseAsync(
                closeStatus: WebSocketCloseStatus.NormalClosure,
                statusDescription: string.Empty, cancellationToken: cancellationToken);

        // Remove connection from online connections 
        Clients.Remove(client);
    }

    public virtual async Task OnReceiveRequestAsync(ConnectionClient client, ConnectionMessage message)
    {
        var handler = Handlers.SingleOrDefault(
            x => x.Name.ToLower().StartsWith(message.Handler.ToLower()));

        if (handler == null || handler.BaseType != typeof(ConnectionHandler))
            return;

        var method = handler.GetMethods().SingleOrDefault(function =>
            function.Name.Equals(message.Method, StringComparison.CurrentCultureIgnoreCase)
                && !typeof(ConnectionHandler).GetMethods().Any(x => x.Name == function.Name));

        if (method == null || method.GetParameters().Count() != message.Parameters.Count())
            return;

        var parameters = message.Parameters.Select(parameter =>
        {
            var parameterMethod = method.GetParameters()
                .SingleOrDefault(x => x.Name == parameter.Key);

            return parameter.Value.Map(parameterMethod.ParameterType);
        });

        await using (var scope = _scopeFactory.CreateAsyncScope())
        {
            var _handler = (ConnectionHandler)
                scope.ServiceProvider.GetRequiredService(handler);

            (_handler.Client, _handler.Message, _handler.Connection)
                = (client, message, this);

            foreach (ConnectionHandlerParameter parameter in parameters.Where(x => x.GetType().IsSubclassOf(typeof(ConnectionHandlerParameter))))
            {
                if (parameter.IsValidRequest() == false)
                {
                    await _handler.BadRequestResponse(parameter.GetValidationResults());
                    return;
                }
            }

            method.Invoke(_handler, parameters: parameters.ToArray());
        }
    }

    public virtual async Task Broadcast(ConnectionMessage message, ConnectionMessageType messageType = ConnectionMessageType.Encrypt, CancellationToken cancellationToken = default)
    {
        foreach (ConnectionClient client in Clients)
            await client.SendAsync(
                message: message, messageType: messageType, cancellationToken: cancellationToken);
    }
}
