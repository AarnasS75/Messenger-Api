using Messenger.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Exception = System.Exception;

namespace Messenger.Infrastructure.Workers;

public class ResendEmailWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _retryInterval = TimeSpan.FromSeconds(60);
    private const int BATCH_SIZE = 10;
    private readonly ILogger<ResendEmailWorker> _logger;    
    
    public ResendEmailWorker(IServiceProvider serviceProvider, ILogger<ResendEmailWorker> logger)
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
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            var failedEmails = queue.PeekFailedEmails(BATCH_SIZE);
            foreach (var message in failedEmails)
            {
                try
                {
                    await emailService.SendAsync(message);
                }
                catch (Exception exception)
                {
                    // If SendAsync throws exception, it adds that message to failed messages list,
                    // so Remove here removes the duplicate
                    queue.Remove(message); 
                    _logger.LogInformation(exception, "Email resend failed");
                }
            }

            await Task.Delay(_retryInterval, stoppingToken);
        }
    }
}