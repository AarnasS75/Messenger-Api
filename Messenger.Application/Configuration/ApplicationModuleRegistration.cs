using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Application.Configuration;

public static class ApplicationModuleRegistration
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationModuleRegistration).Assembly);
        });
        
        return services;
    }
}