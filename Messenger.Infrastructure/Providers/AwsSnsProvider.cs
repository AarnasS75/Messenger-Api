using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Messenger.Application.Common.Interfaces.Providers;
using Messenger.Application.Services.Notification.Commands.Email;
using Messenger.Application.Services.Notification.Commands.Sms;
using Messenger.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Messenger.Infrastructure.Providers;

public class AwsSnsProvider : 
    INotificationProvider<SmsCommand>, 
    INotificationProvider<EmailCommand>
{
    private readonly AmazonSimpleNotificationServiceClient _client;
    private readonly SNSConfiguration _configuration;
    private readonly ILogger<AwsSnsProvider> _logger;
    
    public AwsSnsProvider(IOptions<SNSConfiguration> configuration, ILogger<AwsSnsProvider> logger)
    {
        _logger = logger;
        _configuration = configuration.Value;
        var credentials = new BasicAWSCredentials(_configuration.AccessKey, _configuration.SecretKey);
        _client = new AmazonSimpleNotificationServiceClient(credentials, region: RegionEndpoint.EUCentral1);
    }
    
    public async Task SendAsync(SmsCommand command)
    {
        try
        {
            var smsRequest = new PublishRequest
            {
                PhoneNumber = command.ToPhoneNumber,
                Message = command.Message
            };
            await _client.PublishAsync(smsRequest);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to send Sms to {To}", command.ToPhoneNumber);
            throw;
        }
    }

    public async Task SendAsync(EmailCommand command)
    {
        try
        {
            var emailRequest = new PublishRequest
            {
                TopicArn = _configuration.TopicArn,
                Message = command.Body,
                Subject = command.Subject
            };
            await _client.PublishAsync(emailRequest);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to send Email to {To}", command.Recipient);
            throw;
        }
    }
}