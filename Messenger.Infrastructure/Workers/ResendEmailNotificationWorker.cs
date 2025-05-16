using MediatR;
using Messenger.Application.Common.Interfaces.Persistence;
using Messenger.Application.Services.Notification.Commands.Email;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Messenger.Infrastructure.Workers;

public class ResendEmailNotificationWorker : BackgroundService
{
    private readonly INotificationQueue<EmailCommand> _messageQueue;
    private readonly ISender _sender;
    private readonly TimeSpan _timeSpan = TimeSpan.FromSeconds(15);
    private readonly ILogger<ResendEmailNotificationWorker> _logger;
    private const int BATCH_SIZE = 10;

    public ResendEmailNotificationWorker(
        INotificationQueue<EmailCommand> messageQueue, 
        ISender sender, 
        ILogger<ResendEmailNotificationWorker> logger)
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