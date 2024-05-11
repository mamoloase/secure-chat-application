namespace Protocol.Transport;
public class ConnectionGroup
{
    public string Id { get; set; }
    public List<ConnectionClient> Connections { get; set; } = new();

    public virtual async Task Broadcast(ConnectionMessage message, ConnectionMessageType messageType = ConnectionMessageType.Encrypt, CancellationToken cancellationToken = default)
    {
        // Send a request to the group connections

        foreach (ConnectionClient connection in Connections)
            await connection.SendAsync(
                message: message,
                messageType: messageType,
                cancellationToken: cancellationToken);
    }
}
