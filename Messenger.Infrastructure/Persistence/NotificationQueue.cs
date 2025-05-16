using Messenger.Application.Common.Interfaces.Persistence;

namespace Messenger.Infrastructure.Persistence;

public class NotificationQueue<T> : INotificationQueue<T>
{
    private readonly List<T> _queue = [];

    public void Enqueue(T request) => _queue.Add(request);
    public void Dequeue(T request) => _queue.Remove(request);
    public List<T> Peek(int batchSize) => _queue.Take(batchSize).ToList();
}