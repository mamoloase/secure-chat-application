namespace Protocol.Transport;
public class ConnectionMessage
{
    public string Id { get; set; }
    public string AnswerId { get; set; }

    public string Method { get; set; } = string.Empty;
    public string Handler { get; set; } = string.Empty;
    public List<ConnectionParameter> Headers { get; set; } = new();
    public List<ConnectionParameter> Parameters { get; set; } = new();

    public ConnectionMessage()
    {
        Id = Guid.NewGuid().ToString();
    }
}

