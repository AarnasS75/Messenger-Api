using MassTransit;
using MediatR;
using Message.Handlers.Messages;
using Messenger.Application.Common.Interfaces;
using Messenger.Application.Services.Notification.Commands.Email;
using Messenger.Contracts.Models;
using Microsoft.Extensions.Logging;

namespace Message.Handlers.Contracts;

public class ShipmentCompletedEventHandler : IConsumer<ShipmentCompletedEventMesssage>
{
    private readonly IMediator _mediator;
    private readonly ILogger<ShipmentCompletedEventMesssage> _logger;

    public ShipmentCompletedEventHandler(IMediator mediator, ILogger<ShipmentCompletedEventMesssage> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ShipmentCompletedEventMesssage> context)
    {
        try
        {
            await _mediator.Send(new EmailCommand(
                Recipient: context.Message.Recipient,
                Body: context.Message.Body,
                Subject: context.Message.Body));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "ShipmentCompletedEventHandler failed.");
            throw;
        }
    }
}
