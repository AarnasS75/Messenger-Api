using Messenger.Contracts.Models;

namespace Messenger.Application.Interfaces;

public interface ISmsService
{
    Task SendAsync(SmsNotificationRequest request);
}