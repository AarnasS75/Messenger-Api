using MassTransit;
using MediatR;
using Message.Handlers.Messages;
using Messenger.Application.Services.Notification.Commands.Sms;
using Microsoft.Extensions.Logging;

namespace Message.Handlers.Handlers;

public class UserAuthenticationEventHandler : IConsumer<UserAuthenticatedEventMessage>
{
    private readonly IMediator _mediator;
    private readonly ILogger<ShipmentCompletedEventMesssage> _logger;

    public UserAuthenticationEventHandler(
        IMediator mediator, 
        ILogger<ShipmentCompletedEventMesssage> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserAuthenticatedEventMessage> context)
    {
        try
        {
            await _mediator.Send(new SmsCommand(
                FromPhoneNumber: context.Message.PhoneNumber,
                ToPhoneNumber: context.Message.PhoneNumber,
                Message: context.Message.Payload));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "UserAuthenticationEventHandler failed.");
            throw;
        }
    }
}