using MassTransit;
using Message.Handlers.Contracts.Messages;
using Messenger.Application.Interfaces;
using Messenger.Contracts.Models;
using Microsoft.Extensions.Logging;

namespace Message.Handlers.Contracts;

public class ShipmentCompletedEventHandler : IConsumer<ShipmentCompletedEventMesssage>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<ShipmentCompletedEventMesssage> _logger;

    public ShipmentCompletedEventHandler(ILogger<ShipmentCompletedEventMesssage> logger, IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public async Task Consume(ConsumeContext<ShipmentCompletedEventMesssage> context)
    {
        try
        {
            await _emailService.SendAsync(new EmailNotificationRequest
            {
                Recipient = context.Message.UserName,
                Message = context.Message.ShipmentId,
                Subject = "Shipment Completed"
            });
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "ShipmentCompletedEventHandler failed.");
            throw;
        }
    }
}
