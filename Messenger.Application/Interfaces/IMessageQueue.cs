using Messenger.Application.Models;

namespace Messenger.Application.Interfaces;

public interface IMessageQueue
{
    void Enqueue(NotificationRequest request);
    IEnumerable<NotificationRequest> DequeueFailedMessages(int batchSize);
}