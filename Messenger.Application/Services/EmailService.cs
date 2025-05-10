using Messenger.Application.Configuration;
using Messenger.Application.Interfaces;
using Messenger.Contracts.Models;
using Messenger.Domain.Enums;
using Messenger.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace Messenger.Application.Services;

public class EmailService : IEmailService
{
    private readonly ISNSProvider _isnsProvider;
    private readonly IMessageQueue _messageQueue;
    
    private readonly NotificationProvidersSettings _providersSettings;

    public EmailService(
        ISNSProvider isnsProvider, 
        IMessageQueue messageQueue, 
        IOptions<NotificationProvidersSettings> providersSettings)
    {
        _isnsProvider = isnsProvider;
        _messageQueue = messageQueue;
        _providersSettings = providersSettings.Value;
    }

    public async Task SendAsync(EmailNotificationRequest request)
    {
        if (!_providersSettings.Channels.Any(x => x.Enabled && x.Type == NotificationType.Email))
        {
            _messageQueue.Enqueue(request);
            throw new InvalidOperationException("Email channel is disabled!");
        }
        
        var availableProviders = _providersSettings.Providers
            .Where(p => p.Enabled)
            .OrderBy(p => p.Priority)
            .Select(p => p.Type)
            .ToList();

        foreach (var provider in availableProviders)
        {
            try
            {
                switch (provider)
                {
                    case ProviderType.SNS:
                        await _isnsProvider.SendEmailAsync(request.Recipient, request.Subject, request.Message);
                        break;
                }
            }
            catch
            {
                _messageQueue.Enqueue(request);
            }
        }
    }
}