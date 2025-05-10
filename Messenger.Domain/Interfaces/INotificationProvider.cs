namespace Messenger.Domain.Interfaces;

public interface INotificationProvider
{
    Task SendAsync(string recipient, string subject, string message);
}