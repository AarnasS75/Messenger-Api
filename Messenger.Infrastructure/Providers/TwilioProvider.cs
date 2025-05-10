using Messenger.Domain.Interfaces;
using Messenger.Infrastructure.Configuration;
using Messenger.Infrastructure.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Messenger.Infrastructure.Providers;

public class TwilioProvider : INotificationProvider
{
    private readonly TwilioConfiguration _configuration;
    
    public TwilioProvider(ITwilioConfigurationProvider configurationProvider)
    {
        _configuration = configurationProvider.GetConfigurationAsync().Result;
        TwilioClient.Init(_configuration.AccountSid, _configuration.AuthToken);
    }
    
    public async Task SendAsync(string recipient, string subject, string message)
    {
        await MessageResource.CreateAsync(
            to: new PhoneNumber("+15673717680"),
            from: new PhoneNumber(_configuration.FromPhoneNumber),
            body: message
        );
    }
}