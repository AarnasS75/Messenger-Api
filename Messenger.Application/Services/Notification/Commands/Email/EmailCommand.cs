using MediatR;

namespace Messenger.Application.Services.Notification.Commands.Email;

public record EmailCommand(
    string Recipient, 
    string Subject, 
    string Body) : IRequest;