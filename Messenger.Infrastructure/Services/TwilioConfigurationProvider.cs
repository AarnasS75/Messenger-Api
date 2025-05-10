using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Messenger.Infrastructure.Configuration;
using Messenger.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Messenger.Infrastructure.Services;

public class TwilioConfigurationProvider : ITwilioConfigurationProvider
{
    private readonly IAmazonSecretsManager _secretService;
    private readonly ILogger<TwilioConfigurationProvider> _logger;
    
    public TwilioConfigurationProvider(IAmazonSecretsManager secretService, ILogger<TwilioConfigurationProvider> logger)
    {
        _secretService = secretService;
        _logger = logger;
    }

    public async Task<TwilioConfiguration> GetConfigurationAsync()
    {
        try
        {
            var response = await _secretService.GetSecretValueAsync(new GetSecretValueRequest
            {
                SecretId = "Messenger/Twilio"
            });
        
            return JsonConvert.DeserializeObject<TwilioConfiguration>(response.SecretString);
        }
        catch (ResourceNotFoundException e)
        {
            _logger.LogError(e, "Secret 'Messenger/Twilio' not found. Returning default configuration.");
            return new TwilioConfiguration();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while retrieving SNS configuration.");
            throw;
        }
    }
}