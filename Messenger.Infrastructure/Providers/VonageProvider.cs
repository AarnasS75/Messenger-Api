using Messenger.Application.Common.Interfaces.Providers;
using Messenger.Application.Services.Notification.Commands.Sms;
using Messenger.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vonage;
using Vonage.Messaging;
using Vonage.Request;

namespace Messenger.Infrastructure.Providers;

public class VonageProvider : INotificationProvider<SmsCommand>
{
    private readonly VonageClient _client;
    private readonly VonageConfiguration _configuration;
    private readonly ILogger<VonageProvider> _logger;
    
    public VonageProvider(IOptions<VonageConfiguration> configuration, ILogger<VonageProvider> logger)
    {
        _logger = logger;
        _configuration = configuration.Value;
        
        var credentials = Credentials.FromApiKeyAndSecret(_configuration.ApiKey, _configuration.ApiSecret);
        _client = new VonageClient(credentials);
    }

    public async Task SendAsync(SmsCommand request)
    {
        try
        {
            await _client.SmsClient.SendAnSmsAsync(new SendSmsRequest
            {
                To = request.ToPhoneNumber,
                From = _configuration.CompanyPhoneNumber,
                Text = request.Message
            });
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to send Sms to {To}", request.ToPhoneNumber);
            throw;
        }
    }
}