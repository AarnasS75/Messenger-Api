using Messenger.Application.Common.Interfaces.Providers;
using Messenger.Application.Services.Notification.Commands.Sms;
using Messenger.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Messenger.Infrastructure.Providers;

public class TwilioProvider : INotificationProvider<SmsCommand>
{
    private readonly ILogger<TwilioProvider> _logger;
    private readonly TwilioConfiguration _configuration;
    
    public TwilioProvider(IOptions<TwilioConfiguration> configuration, ILogger<TwilioProvider> logger)
    {
        _logger = logger;
        _configuration = configuration.Value;  
        TwilioClient.Init(_configuration.AccountSid, _configuration.AuthToken);
    }
    
    public async Task SendAsync(SmsCommand request)
    {
        try
        {
            await MessageResource.CreateAsync(
                to: new PhoneNumber(request.ToPhoneNumber),
                from: new PhoneNumber(_configuration.CompanyPhoneNumber),
                body: request.Message
            );
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to send SMS to {To}", request.ToPhoneNumber);
            throw;
        }
    }
}