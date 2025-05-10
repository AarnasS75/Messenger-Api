using Messenger.Contracts.Models;

namespace Messenger.Application.Interfaces;

public interface IEmailService
{
    Task SendAsync(EmailNotificationRequest request);
}