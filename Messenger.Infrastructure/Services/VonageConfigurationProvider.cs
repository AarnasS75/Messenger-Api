using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Messenger.Infrastructure.Configuration;
using Messenger.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Messenger.Infrastructure.Services;

public class VonageConfigurationProvider : IVonageConfigurationProvider
{
    private readonly IAmazonSecretsManager _secretsManager;
    private readonly ILogger<VonageConfigurationProvider> _logger;
    
    public VonageConfigurationProvider(IAmazonSecretsManager secretsManager, ILogger<VonageConfigurationProvider> logger)
    {
        _secretsManager = secretsManager;
        _logger = logger;
    }

    public async Task<VonageConfiguration> GetConfigurationAsync()
    {
        try
        {
            var response = await _secretsManager.GetSecretValueAsync(new GetSecretValueRequest
            {
                SecretId = "Messenger/Vonage"
            });
        
            return JsonConvert.DeserializeObject<VonageConfiguration>(response.SecretString);
        }
        catch (ResourceNotFoundException e)
        {
            _logger.LogError(e, "Secret 'Messenger/Vonage' not found. Returning default configuration.");
            return new VonageConfiguration();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while retrieving SNS configuration.");
            throw;
        }
    }
}