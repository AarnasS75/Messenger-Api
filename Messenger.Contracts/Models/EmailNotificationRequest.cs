namespace Messenger.Contracts.Models;

public class EmailNotificationRequest
{
    public string Recipient { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
}