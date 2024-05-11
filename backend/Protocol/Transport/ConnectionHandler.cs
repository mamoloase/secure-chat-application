using Protocol.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Protocol.Transport;
public class ConnectionHandler
{
    public Connection Connection { get; set; }
    public ConnectionClient Client { get; set; }
    public ConnectionMessage Message { get; set; }

    public async Task<bool> Response(ConnectionMessage message, ConnectionMessageType messageType = ConnectionMessageType.Encrypt)
    {
        if (string.IsNullOrEmpty(message.Handler))
            message.Handler = "*";

        if (string.IsNullOrEmpty(message.Method))
            message.Method = "Result";

        message.AnswerId = Message.Id;

        await Client.SendAsync(
            message: message, messageType: messageType);

        return true;

    }
    public async Task<bool> Response(string handler = "*", string method = "result", ConnectionMessageType messageType = ConnectionMessageType.Encrypt)
    {
        var message = new ConnectionMessage
        {
            Method = method,
            Handler = handler,
        };

        return await Response(
            message: message, messageType: messageType);
    }
    public async Task<bool> NotFoundResponse(ConnectionMessageType messageType = ConnectionMessageType.Encrypt)
    {
        var message = new ConnectionMessage
        {
            Method = "NotFound",
            Handler = "Exception",
        };

        return await Response(
            message: message, messageType: messageType);
    }

    public async Task<bool> NotFoundResponse(object exception, ConnectionMessageType messageType = ConnectionMessageType.Encrypt)
    {
        var message = new ConnectionMessage
        {
            Method = "NotFound",
            Handler = "Exception",
        };
        message.Parameters.Add("exception", exception);

        return await Response(
            message: message, messageType: messageType);
    }

    public async Task<bool> BadRequestResponse(object exception, ConnectionMessageType messageType = ConnectionMessageType.Encrypt)
    {
        var message = new ConnectionMessage
        {
            Method = "BadRequest",
            Handler = "Exception",
        };
        message.Parameters.Add("exception", exception);

        return await Response(
            message: message, messageType: messageType);
    }
    public async Task<bool> BadRequestResponse(ConnectionMessageType messageType = ConnectionMessageType.Encrypt)
    {
        var message = new ConnectionMessage
        {
            Method = "BadRequest",
            Handler = "Exception",
        };

        return await Response(
            message: message, messageType: messageType);
    }

    public async Task<bool> BadRequestResponse(List<ValidationResult> validationResults, ConnectionMessageType messageType = ConnectionMessageType.Encrypt)
    {
        var message = new ConnectionMessage();
        var exception = new Dictionary<string, List<string>>();

        foreach (var validationResult in validationResults)
        {
            foreach (var memberName in validationResult.MemberNames)
            {
                if (exception.ContainsKey(memberName) == false)
                    exception.Add(memberName, new());

                exception.SingleOrDefault(x => x.Key == memberName).Value
                    .Add(validationResult.ErrorMessage);
            }
        }

        message.Method = "BadRequest";
        message.Handler = "Exception";

        message.Parameters.Add("exception", exception);

        return await Response(
            message: message, messageType: messageType);
    }

    public async Task Broadcase(ConnectionMessage message, ConnectionMessageType messageType = ConnectionMessageType.Encrypt)
        => await Connection.Broadcast(message: message, messageType: messageType);
}
