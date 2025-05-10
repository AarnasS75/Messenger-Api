using Messenger.Domain.Interfaces;
using Messenger.Infrastructure.Interfaces;
using Vonage;
using Vonage.Messaging;
using Vonage.Request;

namespace Messenger.Infrastructure.Providers;

public class VonageProvider : IVonageProvider
{
    private readonly VonageClient _client;

    public VonageProvider(IVonageConfigurationProvider configuration)
    {
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
            Console.WriteLine($"Failed to send Vonage: {exception}");
            throw;
        }
    }
}