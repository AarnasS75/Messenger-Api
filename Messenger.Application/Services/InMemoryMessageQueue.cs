using System.Collections.Concurrent;
using Messenger.Application.Interfaces;
using Messenger.Contracts.Models;

namespace Messenger.Application.Services;

public class InMemoryMessageQueue : IMessageQueue
{
    private readonly ConcurrentQueue<EmailNotificationRequest> _failedEmails = new();
    private readonly ConcurrentQueue<SmsNotificationRequest> _failedSms = new();

    public void Enqueue(EmailNotificationRequest request)
    {
        _failedEmails.Enqueue(request);
    }

    public void Enqueue(SmsNotificationRequest request)
    {
        _failedSms.Enqueue(request);
    }

    public IEnumerable<EmailNotificationRequest> DequeueFailedEmails(int batchSize)
    {
        var messages = new List<EmailNotificationRequest>();
        for (var i = 0; i < batchSize && _failedEmails.TryDequeue(out var message); i++)
        {
            messages.Add(message);
        }

        return messages;
    }

    public IEnumerable<SmsNotificationRequest> DequeueFailedSms(int batchSize)
    {
        var smss = new List<SmsNotificationRequest>();
        for (var i = 0; i < batchSize && _failedSms.TryDequeue(out var sms); i++)
        {
            smss.Add(sms);
        }

        return smss;
    }
}