using Messenger.Application.Interfaces;
using Messenger.Domain.Enums;
using Messenger.Domain.Interfaces;
using Messenger.Infrastructure.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Infrastructure.Factory;

public class NotificationProviderFactory : INotificationProviderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public NotificationProviderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ISmsProvider GetSmsProvider(ProviderType name)
    {
        return name switch
        {
            ProviderType.Twilio => _serviceProvider.GetRequiredService<TwilioProvider>(),
            ProviderType.Aws_SNS => _serviceProvider.GetRequiredService<AwsSnsProvider>(),
            ProviderType.Vonage => _serviceProvider.GetRequiredService<VonageProvider>(),
            _ => throw new ArgumentException($"SMS provider '{name}' not supported.")
        };
    }

    public IEmailProvider GetEmailProvider(ProviderType name)
    {
        return name switch
        {
            ProviderType.Aws_SNS => _serviceProvider.GetRequiredService<AwsSnsProvider>(),
            _ => throw new ArgumentException($"Email provider '{name}' not supported.")
        };
    }
}