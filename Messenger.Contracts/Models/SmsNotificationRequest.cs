namespace Messenger.Contracts.Models;

public class SmsNotificationRequest
{
    public string FromPhoneNumber { get; set; }
    public string ToPhoneNumber { get; set; }
    public string Message { get; set; }
}