using MediatR;
using Messenger.Application.Common.Interfaces.Providers;
using Messenger.Application.Services.Notification.Commands.Email;
using Messenger.Application.Services.Notification.Commands.Sms;
using Messenger.Domain.Enums;
using Messenger.Infrastructure.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Infrastructure.Factory;

public class NotificationProviderFactory : INotificationProviderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public NotificationProviderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public INotificationProvider<TCommand>? GetProvider<TCommand>(ProviderType providerType) where TCommand : IRequest
    {
        return typeof(TCommand).Name switch
        {
            // Sms Providers
            nameof(SmsCommand) => providerType switch
            {
                ProviderType.Aws_SNS => _serviceProvider.GetRequiredService<AwsSnsProvider>() as
                    INotificationProvider<TCommand>,
                ProviderType.Twilio => _serviceProvider.GetRequiredService<TwilioProvider>() as
                    INotificationProvider<TCommand>,
                ProviderType.Vonage => _serviceProvider.GetRequiredService<VonageProvider>() as
                    INotificationProvider<TCommand>,
                _ => throw new Exception($"SMS provider: {providerType} was not found")
            },
            // Email Providers
            nameof(EmailCommand) => providerType switch
            {
                ProviderType.Aws_SNS => _serviceProvider.GetRequiredService<AwsSnsProvider>() as
                    INotificationProvider<TCommand>,
                _ => throw new Exception($"Email provider: {providerType} was not found")
            },
            _ => throw new Exception($"{typeof(TCommand).Name} was not found")
        };
    }
}