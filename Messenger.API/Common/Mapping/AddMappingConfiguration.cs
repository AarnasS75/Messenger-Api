using System.Reflection;
using Mapster;
using MapsterMapper;

namespace Messenger.API.Common.Mapping;

public static class AddMappingConfiguration
{
    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        var cfg = TypeAdapterConfig.GlobalSettings;
        cfg.Scan(Assembly.GetExecutingAssembly());
        
        services.AddSingleton(cfg);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }
}