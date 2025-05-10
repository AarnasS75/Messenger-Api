using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Messenger.Domain.Interfaces;
using Messenger.Infrastructure.Configuration;
using Messenger.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Messenger.Infrastructure.Providers;

public class SNSProvider : ISNSProvider
{
    private readonly AmazonSimpleNotificationServiceClient _client;
    private readonly SNSConfiguration _snsConfiguration;
    private readonly ILogger<SNSProvider> _logger;
    
    public SNSProvider(ISNSConfigurationProvider configurationProvider, ILogger<SNSProvider> logger)
    {
        _logger = logger;
        _snsConfiguration = configurationProvider.GetConfigurationAsync().Result;
        
        var credentials = new BasicAWSCredentials(_snsConfiguration.AccessKey, _snsConfiguration.SecretKey);
        _client = new AmazonSimpleNotificationServiceClient(credentials, region: RegionEndpoint.EUCentral1);
    }
    
    public async Task SendEmailAsync(string recipient, string subject, string message)
    {
        try
        {
            var emailRequest = new PublishRequest
            {
                TopicArn = _snsConfiguration.TopicArn,
                Message = message,
                Subject = subject
            };
            await _client.PublishAsync(emailRequest);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to send Email to {To}", recipient);
            throw;
        }
    }

    public async Task SendSmsAsync(string phoneNumber, string message)
    {
        try
        {
            var smsRequest = new PublishRequest
            {
                PhoneNumber = phoneNumber,
                Message = message
            };
            await _client.PublishAsync(smsRequest);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to send Sms to {To}", phoneNumber);
            throw;
        }
    }
}