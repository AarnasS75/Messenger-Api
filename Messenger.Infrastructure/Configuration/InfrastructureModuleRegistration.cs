using Amazon;
using Amazon.SecretsManager;
using Messenger.Application.Interfaces;
using Messenger.Application.Services;
using Messenger.Domain.Interfaces;
using Messenger.Infrastructure.Interfaces;
using Messenger.Infrastructure.Providers;
using Messenger.Infrastructure.Services;
using Messenger.Infrastructure.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Infrastructure.Configuration;

public static class InfrastructureModuleRegistration
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IAmazonSecretsManager>(new AmazonSecretsManagerClient(region: RegionEndpoint.EUCentral1));
        
        services.AddScoped<ISNSConfigurationProvider, SnsConfigurationProvider>();
        services.AddScoped<ITwilioConfigurationProvider, TwilioConfigurationProvider>();
        services.AddScoped<IVonageConfigurationProvider, VonageConfigurationProvider>();

        services.AddScoped<ISNSProvider, SNSProvider>();
        services.AddScoped<ITwilioProvider, TwilioProvider>();
        services.AddScoped<IVonageProvider, VonageProvider>();
        
        services.AddSingleton<IMessageQueue, InMemoryMessageQueue>();
        services.AddHostedService<ResendSmsWorker>();
        services.AddHostedService<ResendEmailWorker>();

        return services;
    }
}