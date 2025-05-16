namespace Messenger.Application.Common.Interfaces.Persistence;

public interface INotificationQueue<T>
{
    void Enqueue(T request);
    void Dequeue(T request);
    List<T> Peek(int batchSize);
}
