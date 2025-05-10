using Messenger.Application.Models;

namespace Messenger.Application.Interfaces;

public interface INotificationService
{
    Task SendNotificationAsync(NotificationRequest request);
}