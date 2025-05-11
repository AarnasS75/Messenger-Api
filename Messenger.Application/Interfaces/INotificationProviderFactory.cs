using Messenger.Domain.Enums;
using Messenger.Domain.Interfaces;

namespace Messenger.Application.Interfaces;

public interface INotificationProviderFactory
{
    ISmsProvider GetSmsProvider(ProviderType name);
    IEmailProvider GetEmailProvider(ProviderType name);
}