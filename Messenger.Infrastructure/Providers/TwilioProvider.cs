using Messenger.Domain.Interfaces;
using Messenger.Infrastructure.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Messenger.Infrastructure.Providers;

public class TwilioProvider : ITwilioProvider
{
    public TwilioProvider(ITwilioConfigurationProvider configurationProvider)
    {
        var cfg = configurationProvider.GetConfigurationAsync().Result;
        TwilioClient.Init(cfg.AccountSid, cfg.AuthToken);
    }
    
    public async Task SendSmsAsync(string fromPhoneNumber, string toPhoneNumber, string message)
    {
        try
        {
            await MessageResource.CreateAsync(
                to: new PhoneNumber(toPhoneNumber),
                from: new PhoneNumber(fromPhoneNumber),
                body: message
            );
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Failed to send sms: {exception}");
            throw;
        }
    }
}