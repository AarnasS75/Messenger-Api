using MediatR;
using Messenger.Application.Common.Interfaces.Persistence;
using Messenger.Application.Common.Interfaces.Providers;
using Messenger.Application.Configuration;
using Messenger.Domain.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Messenger.Application.Services.Notification.Commands.Base;

public abstract class NotificationCommandHandler<TCommand> : IRequestHandler<TCommand> where TCommand : IRequest
{
    private readonly NotificationsConfiguration _config;
    private readonly INotificationProviderFactory _providerFactory;
    private readonly INotificationQueue<TCommand> _queue;
    private readonly ILogger<NotificationCommandHandler<TCommand>> _logger;

    protected NotificationCommandHandler(
        INotificationProviderFactory providerFactory,
        IOptions<NotificationsConfiguration> config,
        INotificationQueue<TCommand> queue,
        ILogger<NotificationCommandHandler<TCommand>> logger)
    {
        _providerFactory = providerFactory;
        _config = config.Value;
        _queue = queue;
        _logger = logger;
    }


    public async Task Handle(TCommand request, CancellationToken cancellationToken)
    {
        if (!IsChannelEnabled(_config))
        {
            _queue.Enqueue(request);
            throw new Exception($"{typeof(TCommand).Name} channel is not active or disabled!");
        }

        var providers = GetProviders(_config)
            .Where(p => p.Value.Enabled)
            .OrderBy(p => p.Value.Priority)
            .ToList();

        var sent = false;

        foreach (var provider in providers)
        {
            try
            {
                var availableProvider = _providerFactory.GetProvider<TCommand>(provider.Key);
                if (availableProvider == null)
                {
                    continue;
                }
                
                await availableProvider.SendAsync(request);
                
                sent = true;
                break;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"{provider.Key} failed to send {typeof(TCommand).Name}");
            }
        }

        if (!sent)
        {
            _logger.LogError($"Error: Could not send {typeof(TCommand).Name}");
        }
    }
    
    protected abstract bool IsChannelEnabled(NotificationsConfiguration config);
    protected abstract IEnumerable<KeyValuePair<ProviderType, ProviderConfiguration>> GetProviders(NotificationsConfiguration config);
}