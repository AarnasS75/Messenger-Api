using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Messenger.Infrastructure.Configuration;
using Messenger.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Messenger.Infrastructure.Services;

public class SnsConfigurationProvider : ISNSConfigurationProvider
{
    private readonly IAmazonSecretsManager _secretsManager;
    private readonly ILogger<SnsConfigurationProvider> _logger;
    
    public SnsConfigurationProvider(IAmazonSecretsManager secretsManager, ILogger<SnsConfigurationProvider> logger)
    {
        _secretsManager = secretsManager;
        _logger = logger;
    }

    public async Task<SNSConfiguration> GetConfigurationAsync()
    {
        try
        {
            var response = await _secretsManager.GetSecretValueAsync(new GetSecretValueRequest
            {
                SecretId = "Messenger/SNS"
            });
        
            return JsonConvert.DeserializeObject<SNSConfiguration>(response.SecretString);
        }
        catch (ResourceNotFoundException e)
        {
            _logger.LogError(e, "Secret 'Messenger/SNS' not found. Returning default configuration.");
            return new SNSConfiguration();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while retrieving SNS configuration.");
            throw;
        }
    }
}