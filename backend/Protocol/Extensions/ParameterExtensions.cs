using Protocol.Transport;

namespace Protocol.Extensions;
public static class ParameterExtensions
{
    public static void Add(this List<ConnectionParameter> parameters, string key, object value)
    {
        parameters.Add(new ConnectionParameter { Key = key, Value = value });
    }

    public static bool TryGetValue<T>(this List<ConnectionParameter> parameters, string key, out T result)
    {
        result = (T)parameters.SingleOrDefault(x => x.Key == key)?.Value;

        return parameters.Any(x => x.Key == key);
    }
}