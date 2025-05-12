using Messenger.Application.Configuration;
using Messenger.Application.Interfaces;
using Messenger.Contracts.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Messenger.Application.Services;

public class EmailService : IEmailService
{
    private readonly INotificationProviderFactory _providerFactory;
    private readonly IMessageQueue _messageQueue;
    private readonly ILogger<EmailService> _logger;
    
    private readonly NotificationsConfiguration _providersConfiguration;

    public EmailService(
        INotificationProviderFactory providerFactory, 
        IMessageQueue messageQueue, 
        IOptions<NotificationsConfiguration> providersSettings, 
        ILogger<EmailService> logger)
    {
        _providerFactory = providerFactory;
        _messageQueue = messageQueue;
        _logger = logger;
        _providersConfiguration = providersSettings.Value;
    }

    public async Task SendAsync(EmailNotificationRequest request)
    {
        var channel = _providersConfiguration.Email;
        
        if (channel == null || !channel.Enabled)
        {
            _messageQueue.Enqueue(request);
            throw new Exception("Email channel is not active or disabled!");
        }
        
        var availableProviders = channel.Providers
            .Where(kvp => kvp.Value.Enabled)
            .OrderBy(kvp => kvp.Value.Priority)
            .ToList();

        var emailSent = false;
        
        foreach (var availableProvider in availableProviders)
        {
            try
            {
                var provider = _providerFactory.GetEmailProvider(availableProvider.Key);
                await provider.SendEmailAsync(request.Recipient, request.Subject, request.Message);
                
                emailSent = true;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"{availableProvider.Key} failed to send Email");
            }
        }

        if (!emailSent)
        {
            _logger.LogError("Error: Could not send Email notification");
        }
    }
}