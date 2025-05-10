using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Messenger.Infrastructure.Configuration;
using Messenger.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace Messenger.Infrastructure.Services;

public class TwilioConfigurationProvider : ITwilioConfigurationProvider
{
    private readonly IAmazonSecretsManager _secretService;

    public TwilioConfigurationProvider(IAmazonSecretsManager secretService)
    {
        _secretService = secretService;
    }

    public async Task<TwilioConfiguration> GetConfigurationAsync()
    {
        var response = await _secretService.GetSecretValueAsync(new GetSecretValueRequest
        {
            SecretId = "Messenger/Twilio"
        });
        
        return JsonConvert.DeserializeObject<TwilioConfiguration>(response.SecretString);
    }
}