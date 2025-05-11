using Messenger.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Messenger.Infrastructure.Workers;

public class ResendSmsWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _retryInterval = TimeSpan.FromSeconds(60);
    private const int BATCH_SIZE = 10;
    private readonly ILogger<ResendSmsWorker> _logger;
    
    public ResendSmsWorker(IServiceProvider serviceProvider, ILogger<ResendSmsWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var queue = scope.ServiceProvider.GetRequiredService<IMessageQueue>();
            var smsService = scope.ServiceProvider.GetRequiredService<ISmsService>();

            var failedMessages = queue.PeekFailedSms(BATCH_SIZE);
            foreach (var message in failedMessages)
            {
                try
                {
                    await smsService.SendAsync(message);
                }
                catch (Exception exception)
                {
                    // If SendAsync throws exception, it adds that message to failed messages list,
                    // so Remove here removes the duplicate
                    queue.Remove(message);
                    _logger.LogInformation(exception, "SMS resend failed");
                }
            }

            await Task.Delay(_retryInterval, stoppingToken);
        }
    }
}