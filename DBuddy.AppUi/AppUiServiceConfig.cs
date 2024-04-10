using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DBuddy.AppUi;

public static class AppUiServiceConfig
{
    /// <summary>
    /// 注入 AppUi 中的服务（单例）
    /// </summary>
    /// <param name="services">DI 容器</param>
    /// <returns>DI 容器</returns>
    public static IServiceCollection AddSingletonAppUiServices(this IServiceCollection services)
    {
        var assembly = typeof(AppUiServiceConfig).Assembly;
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
                services.TryAddSingleton(baseType, type);
        }

        return services;
    }
}