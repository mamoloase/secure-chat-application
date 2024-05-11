using System.Reflection;

using Protocol.Transport;

using Microsoft.Extensions.DependencyInjection;

namespace Protocol;
public static class DependencyInjection
{
    public static IServiceCollection AddProtocolServices(this IServiceCollection services)
    {
        foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
        {
            if (type.GetTypeInfo().BaseType == typeof(Connection))
                services.AddSingleton(type);
        }
        foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
        {
            if (type.GetTypeInfo().BaseType == typeof(ConnectionHandler))
                services.AddTransient(type);
        }

        return services;
    }
}
