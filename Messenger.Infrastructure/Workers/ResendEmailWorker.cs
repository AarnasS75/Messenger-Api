using Messenger.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Messenger.Infrastructure.Workers;

public class ResendEmailWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _retryInterval = TimeSpan.FromSeconds(60);
    private const int BATCH_SIZE = 10;
    
    public ResendEmailWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var queue = scope.ServiceProvider.GetRequiredService<IMessageQueue>();
            var smsService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            var failedEmails = queue.DequeueFailedEmails(BATCH_SIZE);
            foreach (var message in failedEmails)
            {
                await smsService.SendAsync(message);
            }

            await Task.Delay(_retryInterval, stoppingToken);
        }
    }
}