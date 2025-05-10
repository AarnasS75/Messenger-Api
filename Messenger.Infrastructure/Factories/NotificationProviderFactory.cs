using Messenger.Application.Interfaces;
using Messenger.Domain.Interfaces;
using Messenger.Infrastructure.Configuration;
using Messenger.Infrastructure.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Messenger.Infrastructure.Factories;

public class NotificationProviderFactory : INotificationProviderFactory
{
    private readonly NotificationProvidersSettings _providersSettings;
    private readonly IServiceProvider _serviceProvider;

    public NotificationProviderFactory(
        IOptions<NotificationProvidersSettings> configOptions, 
        IServiceProvider serviceProvider)
    {
        _providersSettings = configOptions.Value;
        _serviceProvider = serviceProvider;
    }

    public List<INotificationProvider> GetProviders()
    {
        var availableProviders = _providersSettings.Providers
            .Where(p => p.Enabled)
            .OrderBy(p => p.Priority)
            .Select(p => p.Type)
            .ToList();
        
        var providers = new List<INotificationProvider>();
        
        foreach (var provider in availableProviders)
        {
            switch (provider)
            {
                case "SNS":
                    providers.Add(_serviceProvider.GetRequiredService<SNSProvider>());
                    break;
                case "Twilio":
                    providers.Add(_serviceProvider.GetRequiredService<TwilioProvider>());
                    break;
                case "Vonage":
                    providers.Add(_serviceProvider.GetRequiredService<VonageProvider>());
                    break;
            }
        }

        return providers;
    }
}