using Messenger.Application.Interfaces;
using Messenger.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Messenger.Infrastructure.Providers;

public class TwilioProvider : ISmsProvider
{
    private readonly ILogger<TwilioProvider> _logger;
    
    public TwilioProvider(ITwilioConfigurationProvider configurationProvider, ILogger<TwilioProvider> logger)
    {
        _logger = logger;
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
            _logger.LogError(exception, "Failed to send SMS to {To}", toPhoneNumber);
            throw;
        }
    }
}