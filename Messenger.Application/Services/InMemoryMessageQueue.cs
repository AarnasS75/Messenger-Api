using System.Collections.Concurrent;
using Messenger.Application.Interfaces;
using Messenger.Application.Models;

namespace Messenger.Application.Services;

public class InMemoryMessageQueue : IMessageQueue
{
    private readonly ConcurrentQueue<NotificationRequest> _failedMessages = new();

    public void Enqueue(NotificationRequest request)
    {
        _failedMessages.Enqueue(request);
    }

    public IEnumerable<NotificationRequest> DequeueFailedMessages(int batchSize)
    {
        var messages = new List<NotificationRequest>();
        for (var i = 0; i < batchSize && _failedMessages.TryDequeue(out var message); i++)
        {
            messages.Add(message);
        }

        return messages;
    }
}