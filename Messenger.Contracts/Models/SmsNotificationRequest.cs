namespace Messenger.Contracts.Models;

public record SmsNotificationRequest(
    string FromPhoneNumber, 
    string ToPhoneNumber, 
    string Message);