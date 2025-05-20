using MediatR;

namespace Messenger.Application.Common.Interfaces.Providers;

public interface INotificationProvider<in TCommand> where TCommand : IRequest
{
    Task SendAsync(TCommand command);
}