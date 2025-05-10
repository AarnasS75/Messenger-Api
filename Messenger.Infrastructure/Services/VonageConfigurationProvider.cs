using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Messenger.Infrastructure.Configuration;
using Messenger.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace Messenger.Infrastructure.Services;

public class VonageConfigurationProvider : IVonageConfigurationProvider
{
    private readonly IAmazonSecretsManager _secretService;

    public VonageConfigurationProvider(IAmazonSecretsManager secretService)
    {
        _secretService = secretService;
    }

    public async Task<VonageConfiguration> GetConfigurationAsync()
    {
        var response = await _secretService.GetSecretValueAsync(new GetSecretValueRequest
        {
            SecretId = "Messenger/Vonage"
        });
        
        return JsonConvert.DeserializeObject<VonageConfiguration>(response.SecretString);
    }
}