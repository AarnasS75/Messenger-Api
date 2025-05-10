using Messenger.Application.Interfaces;
using Messenger.Application.Models;

namespace Messenger.Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationProviderFactory _providerFactory;
    private readonly IMessageQueue _messageQueue;

    public NotificationService(INotificationProviderFactory providerFactory, IMessageQueue messageQueue)
    {
        _providerFactory = providerFactory;
        _messageQueue = messageQueue;
    }

    public async Task SendNotificationAsync(NotificationRequest request)
    {
        var notificationSent = false;
        var providers = _providerFactory.GetProviders();

        foreach (var provider in providers)
        {
            try
            {
                await provider.SendAsync(request.Recipient, request.Subject, request.Message);
                notificationSent = true;
                
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        if (!notificationSent)
        {
            _messageQueue.Enqueue(request);
            throw new InvalidOperationException("All providers failed, notification will be retried later.");
        }
    }
}