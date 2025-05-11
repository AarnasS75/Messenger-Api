using Messenger.Application.Interfaces;
using Messenger.Contracts.Models;

namespace Messenger.Application.Services;

public class InMemoryMessageQueue : IMessageQueue
{
    private readonly List<EmailNotificationRequest> _failedEmails = [];
    private readonly List<SmsNotificationRequest> _failedSms = [];

    public void Enqueue(EmailNotificationRequest request)
    {
        _failedEmails.Add(request);
    }

    public void Enqueue(SmsNotificationRequest request)
    {
        _failedSms.Add(request);
    }
    
    public IEnumerable<EmailNotificationRequest> PeekFailedEmails(int batchSize)
    {
        return _failedEmails.Take(batchSize).ToList();
    }
    
    public IEnumerable<SmsNotificationRequest> PeekFailedSms(int batchSize)
    {
        return _failedSms.Take(batchSize).ToList();
    }
    
    public void Remove(EmailNotificationRequest request)
    {
        _failedEmails.Remove(request);
    }
    
    public void Remove(SmsNotificationRequest request)
    {
        _failedSms.Remove(request);
    }
}