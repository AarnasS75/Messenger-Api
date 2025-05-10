using Messenger.Application.Configuration;
using Messenger.Application.Interfaces;
using Messenger.Contracts.Models;
using Messenger.Domain.Enums;
using Messenger.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Messenger.Application.Services;

public class EmailService : IEmailService
{
    private readonly ISNSProvider _isnsProvider;
    private readonly IMessageQueue _messageQueue;
    private readonly ILogger<EmailService> _logger;
    
    private readonly NotificationProvidersSettings _providersSettings;

    public EmailService(
        ISNSProvider isnsProvider, 
        IMessageQueue messageQueue, 
        IOptions<NotificationProvidersSettings> providersSettings, 
        ILogger<EmailService> logger)
    {
        _isnsProvider = isnsProvider;
        _messageQueue = messageQueue;
        _logger = logger;
        _providersSettings = providersSettings.Value;
    }

    public async Task SendAsync(EmailNotificationRequest request)
    {
        if (!_providersSettings.Channels.Any(x => x.Enabled && x.Type == NotificationType.Email))
        {
            _messageQueue.Enqueue(request);
            throw new Exception("Email channel is disabled!");
        }
        
        var availableProviders = _providersSettings.Providers
            .Where(p => p.Enabled)
            .OrderBy(p => p.Priority)
            .Select(p => p.Type)
            .ToList();

        var emailSent = false;
        
        foreach (var provider in availableProviders)
        {
            try
            {
                switch (provider)
                {
                    case ProviderType.SNS:
                        await _isnsProvider.SendEmailAsync(request.Recipient, request.Subject, request.Message);
                        emailSent = true;
                        break;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"{provider} failed to send Email");
            }
        }

        if (!emailSent)
        {
            _logger.LogError("Error: Could not send Email notification");
        }
    }
}