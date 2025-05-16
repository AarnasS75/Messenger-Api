using MediatR;

namespace Messenger.Application.Services.Notification.Commands.Sms;

public record SmsCommand(
    string FromPhoneNumber, 
    string ToPhoneNumber, 
    string Message) : IRequest;