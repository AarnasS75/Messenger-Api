using Messenger.Domain.Interfaces;
using Messenger.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Vonage;
using Vonage.Messaging;
using Vonage.Request;

namespace Messenger.Infrastructure.Providers;

public class VonageProvider : IVonageProvider
{
    private readonly VonageClient _client;
    private readonly ILogger<VonageProvider> _logger;
    public VonageProvider(IVonageConfigurationProvider configuration, ILogger<VonageProvider> logger)
    {
        _logger = logger;
        var cfg = configuration.GetConfigurationAsync().Result;
        
        var credentials = Credentials.FromApiKeyAndSecret(cfg.ApiKey, cfg.ApiSecret);
        _client = new VonageClient(credentials);
    }

    public async Task SendSmsAsync(string fromPhoneNumber, string toPhoneNumber, string message)
    {
        try
        {
            await _client.SmsClient.SendAnSmsAsync(new SendSmsRequest
            {
                To = toPhoneNumber,
                From = fromPhoneNumber,
                Text = message
            });
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to send Sms to {To}", toPhoneNumber);
            throw;
        }
    }
}