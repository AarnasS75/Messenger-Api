using Messenger.Application.Configuration;
using Messenger.Application.Interfaces;
using Messenger.Contracts.Models;
using Messenger.Domain.Enums;
using Messenger.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Messenger.Application.Services;

public class SmsService : ISmsService
{
    private readonly ITwilioProvider _twilioProvider;
    private readonly ISNSProvider _isnsProvider;
    private readonly IVonageProvider _vonageProvider;
    private readonly ILogger<SmsService> _logger;
    
    private readonly IMessageQueue _messageQueue;
    private readonly NotificationProvidersSettings _providersSettings;

    public SmsService(
        ITwilioProvider twilioProvider, 
        ISNSProvider isnsProvider, 
        IVonageProvider vonageProvider, 
        IMessageQueue messageQueue, 
        IOptions<NotificationProvidersSettings> providersSettings, ILogger<SmsService> logger)
    {
        _twilioProvider = twilioProvider;
        _isnsProvider = isnsProvider;
        _vonageProvider = vonageProvider;
        _messageQueue = messageQueue;
        _logger = logger;
        _providersSettings = providersSettings.Value;
    }

    public async Task SendAsync(SmsNotificationRequest request)
    {
        if (!_providersSettings.Channels.Any(x => x.Enabled && x.Type == NotificationType.SMS))
        {
            _messageQueue.Enqueue(request);
            throw new Exception("SMS channel is disabled!");
        }
        
        var availableProviders = _providersSettings.Providers
            .Where(p => p.Enabled)
            .OrderBy(p => p.Priority)
            .Select(p => p.Type)
            .ToList();

        var smsSent = false;
        
        foreach (var provider in availableProviders)
        {
            try
            {
                switch (provider)
                {
                    case ProviderType.SNS:
                        await _isnsProvider.SendSmsAsync(request.ToPhoneNumber,request.Message);
                        smsSent = true;
                        break;
                    
                    case ProviderType.Twilio:
                        await _twilioProvider.SendSmsAsync(request.FromPhoneNumber, request.ToPhoneNumber, request.Message);
                        smsSent = true;
                        break;
                    
                    case ProviderType.Vonage:
                        await _vonageProvider.SendSmsAsync(request.FromPhoneNumber, request.ToPhoneNumber, request.Message);
                        smsSent = true;
                        break;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"{provider} failed to send sms notification");
            }
        }

        if (!smsSent)
        {
            _logger.LogError("Error: Could not send sms notification");
        }
    }
}