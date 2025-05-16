using MediatR;
using Messenger.Application.Common.Interfaces.Persistence;
using Messenger.Application.Services.Notification.Commands.Sms;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Messenger.Infrastructure.Workers;

public class ResendSmsNotificationWorker : BackgroundService
{
    private readonly INotificationQueue<SmsCommand> _messageQueue;
    private readonly ISender _sender;
    private readonly TimeSpan _timeSpan = TimeSpan.FromSeconds(15);
    private readonly ILogger<ResendSmsNotificationWorker> _logger;
    private const int BATCH_SIZE = 10;

    public ResendSmsNotificationWorker(
        INotificationQueue<SmsCommand> messageQueue, 
        ISender sender, 
        ILogger<ResendSmsNotificationWorker> logger)
    {
        _messageQueue = messageQueue;
        _sender = sender;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var messages = _messageQueue.Peek(BATCH_SIZE);

            foreach (var message in messages)
            {
                try
                {
                    await _sender.Send(message, cancellationToken);
                }
                catch (Exception exception)
                {
                    _messageQueue.Dequeue(message);
                    _logger.LogInformation(exception, "Failed to resend the message");
                }
            }
            
            await Task.Delay(_timeSpan, cancellationToken);
        }
    }
}