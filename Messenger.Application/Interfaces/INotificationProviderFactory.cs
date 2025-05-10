using Messenger.Domain.Interfaces;

namespace Messenger.Application.Interfaces;

public interface INotificationProviderFactory
{
    List<INotificationProvider> GetProviders();
}