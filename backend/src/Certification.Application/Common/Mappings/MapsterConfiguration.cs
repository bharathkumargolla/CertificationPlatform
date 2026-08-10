using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Certification.Application.Common.Mappings;

public static class MapsterConfiguration
{
    public static IServiceCollection AddMapsterConfiguration(this IServiceCollection services, Assembly assembly)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(assembly);

        services.AddSingleton(typeAdapterConfig);

        return services;
    }
}
