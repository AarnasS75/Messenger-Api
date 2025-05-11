using Messenger.Contracts.Models;

namespace Messenger.Application.Interfaces;

public interface IMessageQueue
{
    void Enqueue(EmailNotificationRequest request);
    void Enqueue(SmsNotificationRequest request);
    void Remove(EmailNotificationRequest request);
    void Remove(SmsNotificationRequest request);
    IEnumerable<EmailNotificationRequest> PeekFailedEmails(int batchSize);
    IEnumerable<SmsNotificationRequest> PeekFailedSms(int batchSize);
}