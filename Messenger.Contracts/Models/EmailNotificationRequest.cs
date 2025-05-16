namespace Messenger.Contracts.Models;

public record EmailNotificationRequest(
    string Recipient, 
    string Subject, 
    string Body);