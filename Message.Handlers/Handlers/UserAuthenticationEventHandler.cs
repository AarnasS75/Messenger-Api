using MassTransit;
using Message.Handlers.Contracts.Messages;
using Messenger.Application.Interfaces;
using Messenger.Contracts.Models;
using Microsoft.Extensions.Logging;

namespace Message.Handlers.Handlers;

public class UserAuthenticationEventHandler : IConsumer<UserAuthenticatedEventMessage>
{
    private readonly ISmsService _smsService;
    private readonly ILogger<UserAuthenticationEventHandler> _logger;

    public UserAuthenticationEventHandler(ILogger<UserAuthenticationEventHandler> logger, ISmsService smsService)
    {
        _logger = logger;
        _smsService = smsService;
    }

    public async Task Consume(ConsumeContext<UserAuthenticatedEventMessage> context)
    {
        try
        {
            await _smsService.SendAsync(new SmsNotificationRequest
            {
                FromPhoneNumber = "+37022222222",
                ToPhoneNumber = context.Message.PhoneNumber,
                Message = context.Message.PhoneNumber
            });
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "UserAuthenticationEventHandler failed.");
            throw;
        }
    }
}