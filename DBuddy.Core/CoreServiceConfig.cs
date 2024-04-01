using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DBuddy.Service;

public static class CoreServiceConfig
{
    /// <summary>
    /// 注入 CoreService 中的服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        var assembly = typeof(CoreServiceConfig).Assembly;
        var types = assembly
            .GetTypes()
            .Where(t =>
                !t.IsGenericType &&
                !t.IsAbstract &&
                t.IsClass &&
                t.Name.EndsWith("Service"))
            .ToList();

        foreach (var type in types)
        {
            var baseType = type.GetInterfaces().FirstOrDefault(t => t.Name == $"I{type.Name}");
            if (baseType != null)
                services.TryAddScoped(baseType, type);
        }

        return services;
    }
}