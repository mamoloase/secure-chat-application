using System.Text.Json;

namespace Protocol.Serializer;
public static class SerializerConfiguration
{
    public static JsonSerializerOptions DefaultConfiguration = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
}
