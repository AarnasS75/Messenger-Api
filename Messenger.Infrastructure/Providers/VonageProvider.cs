using Messenger.Domain.Interfaces;
using Messenger.Infrastructure.Configuration;
using Messenger.Infrastructure.Interfaces;
using Vonage;
using Vonage.Messaging;
using Vonage.Request;

namespace Messenger.Infrastructure.Providers;

public class VonageProvider : INotificationProvider
{
    private readonly VonageConfiguration _configuration;

    public VonageProvider(IVonageConfigurationProvider configuration)
    {
        _configuration = configuration.GetConfigurationAsync().Result;
    }

    public async Task SendAsync(string recipient, string subject, string message)
    {
        var credentials = Credentials.FromApiKeyAndSecret(_configuration.ApiKey, _configuration.ApiSecret);
        var client = new VonageClient(credentials);

        var response = await client.SmsClient.SendAnSmsAsync(new SendSmsRequest
        {
            To = recipient,
            From = _configuration.FromNumber,
            Text = message
        });

        if (response.Messages[0].Status != "0")
        {
            throw new InvalidOperationException($"Vonage send failed: {response.Messages[0].ErrorText}");
        }
    }
}