using Messenger.Application.Configuration;
using Messenger.Application.Interfaces;
using Messenger.Contracts.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Messenger.Application.Services;

public class SmsService : ISmsService
{
    private readonly INotificationProviderFactory _providerFactory;
    private readonly IMessageQueue _messageQueue;
    private readonly NotificationsConfiguration _providersConfiguration;

    private readonly ILogger<SmsService> _logger;

    public SmsService(
        IMessageQueue messageQueue, 
        IOptions<NotificationsConfiguration> providersSettings, 
        ILogger<SmsService> logger, 
        INotificationProviderFactory providerFactory)
    {
        _messageQueue = messageQueue;
        _logger = logger;
        _providerFactory = providerFactory;
        _providersConfiguration = providersSettings.Value;
    }

    public async Task SendAsync(SmsNotificationRequest request)
    {
        var channel = _providersConfiguration.Sms;
        
        if (channel == null || !channel.Enabled)
        {
            _messageQueue.Enqueue(request);
            throw new Exception("SMS channel is not active or disabled!");
        }
        
        var availableProviders = channel.Providers
            .Where(kvp => kvp.Value.Enabled)
            .OrderBy(kvp => kvp.Value.Priority)
            .ToList();

        var smsSent = false;
        
        foreach (var availableProvider in availableProviders)
        {
            try
            {
                var provider = _providerFactory.GetSmsProvider(availableProvider.Key);
                await provider.SendSmsAsync(request.FromPhoneNumber, request.ToPhoneNumber, request.Message);
                
                smsSent = true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"{availableProvider.Key} failed to send sms notification");
            }
        }

        if (!smsSent)
        {
            _logger.LogError("Error: Could not send sms notification");
        }
    }
}