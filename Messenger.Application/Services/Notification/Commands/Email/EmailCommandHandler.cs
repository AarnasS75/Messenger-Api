using Messenger.Application.Common.Interfaces.Persistence;
using Messenger.Application.Common.Interfaces.Providers;
using Messenger.Application.Configuration;
using Messenger.Application.Services.Notification.Commands.Base;
using Messenger.Domain.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Messenger.Application.Services.Notification.Commands.Email;

public class EmailCommandHandler : NotificationCommandHandler<EmailCommand>
{
    public EmailCommandHandler(
        INotificationProviderFactory providerFactory, 
        IOptions<NotificationsConfiguration> config, 
        INotificationQueue<EmailCommand> queue, 
        ILogger<EmailCommandHandler> logger) : base(providerFactory, config, queue, logger)
    {
    }

    protected override bool IsChannelEnabled(NotificationsConfiguration config)
    {
        return config.Email.Enabled;
    }

    protected override IEnumerable<KeyValuePair<ProviderType, ProviderConfiguration>> GetProviders(NotificationsConfiguration config)
    {
        return config.Email.Providers;
    }
}