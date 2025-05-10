using Amazon;
using Amazon.SecretsManager;
using Messenger.Application.Interfaces;
using Messenger.Application.Services;
using Messenger.Infrastructure.Factories;
using Messenger.Infrastructure.Interfaces;
using Messenger.Infrastructure.Providers;
using Messenger.Infrastructure.Services;
using Messenger.Infrastructure.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Infrastructure.Configuration;

public static class InfrastructureModuleRegistration
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<NotificationProvidersSettings>(configuration.GetSection(NotificationProvidersSettings.SectionName));

        services.AddSingleton<IAmazonSecretsManager>(new AmazonSecretsManagerClient(region: RegionEndpoint.EUCentral1));
        
        services.AddScoped<ISNSConfigurationProvider, ISnsConfigurationProvider>();
        services.AddScoped<ITwilioConfigurationProvider, TwilioConfigurationProvider>();
        services.AddScoped<IVonageConfigurationProvider, VonageConfigurationProvider>();

        services.AddScoped<INotificationProviderFactory, NotificationProviderFactory>();
        
        services.AddScoped<SNSProvider>();
        services.AddScoped<TwilioProvider>();
        services.AddScoped<VonageProvider>();
        
        services.AddSingleton<IMessageQueue, InMemoryMessageQueue>();
        services.AddHostedService<ResendNotificationWorker>();

        return services;
    }
}