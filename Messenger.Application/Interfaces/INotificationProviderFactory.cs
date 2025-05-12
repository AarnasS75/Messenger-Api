using Messenger.Domain.Enums;

namespace Messenger.Application.Interfaces;

public interface INotificationProviderFactory
{
    ISmsProvider GetSmsProvider(ProviderType name);
    IEmailProvider GetEmailProvider(ProviderType name);
}