using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Messenger.Domain.Interfaces;
using Messenger.Infrastructure.Configuration;
using Messenger.Infrastructure.Interfaces;

namespace Messenger.Infrastructure.Providers;

public class SNSProvider : ISNSProvider
{
    private readonly AmazonSimpleNotificationServiceClient _client;
    private readonly SNSConfiguration _snsConfiguration;

    public SNSProvider(ISNSConfigurationProvider configurationProvider)
    {
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
            Console.WriteLine($"Failed to send SNS Email: {exception}");
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
            Console.WriteLine($"Failed to send SNS Sms: {exception}");
            throw;
        }
    }
}