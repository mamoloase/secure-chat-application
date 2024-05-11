namespace Protocol.Transport.Managers;
public class ConnectionGroupManager
{
    public List<ConnectionGroup> Groups { get; set; }

    public void InsertGroup(ConnectionClient client, string id)
    {
        if (Groups.Any(x => x.Id == id))
            Groups.SingleOrDefault(x => x.Id == id).Connections.Add(client);
        else
            Groups.Add(
                new ConnectionGroup { Id = id, Connections = new() { client } });
    }

    public void RemoveGroup(ConnectionClient client, string id)
    {
        if (Groups.Any(x => x.Id == id))
            if (Groups.SingleOrDefault(x => x.Id == id).Connections.Contains(client))
                Groups.SingleOrDefault(x => x.Id == id).Connections.Remove(client);
    }
    public List<ConnectionClient> GetConnections(string id)
    {
        if (!Groups.Any(x => x.Id == id))
            return new List<ConnectionClient>();

        return Groups.SingleOrDefault(x => x.Id == id).Connections;
    }
}
