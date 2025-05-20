using MediatR;
using Messenger.Domain.Enums;

namespace Messenger.Application.Common.Interfaces.Providers;

public interface INotificationProviderFactory
{
    INotificationProvider<TCommand>? GetProvider<TCommand>(ProviderType providerType)
        where TCommand : IRequest;
}