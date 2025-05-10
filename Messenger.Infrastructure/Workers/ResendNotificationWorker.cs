using Messenger.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Messenger.Infrastructure.Workers;

public class ResendNotificationWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _retryInterval = TimeSpan.FromSeconds(60);
    private const int BATCH_SIZE = 10;
    
    public ResendNotificationWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var queue = scope.ServiceProvider.GetRequiredService<IMessageQueue>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            var failedMessages = queue.DequeueFailedMessages(BATCH_SIZE);
            foreach (var message in failedMessages)
            {
                await notificationService.SendNotificationAsync(message);
            }

            await Task.Delay(_retryInterval, stoppingToken);
        }
    }
}