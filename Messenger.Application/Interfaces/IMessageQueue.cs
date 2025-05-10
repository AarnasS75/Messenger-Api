using Messenger.Contracts.Models;

namespace Messenger.Application.Interfaces;

public interface IMessageQueue
{
    void Enqueue(EmailNotificationRequest request);
    void Enqueue(SmsNotificationRequest request);
    IEnumerable<EmailNotificationRequest> DequeueFailedEmails(int batchSize);
    IEnumerable<SmsNotificationRequest> DequeueFailedSms(int batchSize);
}