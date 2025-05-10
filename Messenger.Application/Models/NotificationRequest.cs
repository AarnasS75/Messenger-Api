namespace Messenger.Application.Models;

public class NotificationRequest
{
    public string Recipient { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
}