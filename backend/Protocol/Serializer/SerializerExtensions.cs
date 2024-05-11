using System.Text.Json;

namespace Protocol.Serializer;
public static class SerializerExtensions
{
    public static string Serialize(this object value, JsonSerializerOptions options = default)
    {
        if (options == null)
            options = SerializerConfiguration.DefaultConfiguration;

        try
        {
            return JsonSerializer.Serialize(value: value, options: options);
        }
        catch { return string.Empty; }
    }
    public static T Deserialize<T>(this string value, JsonSerializerOptions options = default)
    {
        if (options == null)
            options = SerializerConfiguration.DefaultConfiguration;

        try
        {
            return JsonSerializer.Deserialize<T>(json: value, options: options);
        }
        catch { return default; }
    }
    public static object Deserialize(this string value, Type type, JsonSerializerOptions options = default)
    {
        if (options == null)
            options = SerializerConfiguration.DefaultConfiguration;
        try
        {
            return JsonSerializer.Deserialize(json: value, returnType: type, options: options);
        }
        catch { return default; }
    }

    public static object Map(this object value, Type type, JsonSerializerOptions options = default)
    {
        if (options == null)
            options = SerializerConfiguration.DefaultConfiguration;

        try
        {
            return JsonSerializer.Deserialize(
                json: value.Serialize(options), returnType: type, options: options);
        }
        catch { return default; }
    }
    public static T Map<T>(this object value, JsonSerializerOptions options = default)
    {
        if (options == null)
            options = SerializerConfiguration.DefaultConfiguration;

        try
        {
            return JsonSerializer.Deserialize<T>(
                json: value.Serialize(options), options: options);
        }
        catch { return default; }
    }

}
