using Amazon;
using Amazon.SecretsManager;
using Messenger.Application.Common.Interfaces.Authentication;
using Messenger.Application.Common.Interfaces.Persistence;
using Messenger.Application.Common.Interfaces.Providers;
using Messenger.Application.Services.Notification.Commands.Email;
using Messenger.Application.Services.Notification.Commands.Sms;
using Messenger.Infrastructure.Authentication;
using Messenger.Infrastructure.Authentication.Models;
using Messenger.Infrastructure.Factory;
using Messenger.Infrastructure.Persistence;
using Messenger.Infrastructure.Providers;
using Messenger.Infrastructure.Services;
using Messenger.Infrastructure.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Infrastructure.Configuration;

public static class InfrastructureModuleRegistration
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.Configure<SNSConfiguration>(configuration.GetSection(nameof(SNSConfiguration)));
        services.Configure<TwilioConfiguration>(configuration.GetSection(nameof(TwilioConfiguration)));
        services.Configure<VonageConfiguration>(configuration.GetSection(nameof(VonageConfiguration)));

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddSingleton<INotificationQueue<SmsCommand>, NotificationQueue<SmsCommand>>();
        services.AddSingleton<INotificationQueue<EmailCommand>, NotificationQueue<EmailCommand>>();
        
        services.AddScoped<INotificationProviderFactory, NotificationProviderFactory>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<TwilioProvider>();
        services.AddScoped<VonageProvider>();
        services.AddScoped<AwsSnsProvider>();
        
        services.AddHostedService<ResendEmailNotificationWorker>();
        
        return services;
    }
}