using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Messenger.Infrastructure.Configuration;
using Messenger.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace Messenger.Infrastructure.Services;

public class ISnsConfigurationProvider : ISNSConfigurationProvider
{
    private readonly IAmazonSecretsManager _secretsManager;

    public ISnsConfigurationProvider(IAmazonSecretsManager secretsManager)
    {
        _secretsManager = secretsManager;
    }

    public async Task<SNSConfiguration> GetConfigurationAsync()
    {
        var response = await _secretsManager.GetSecretValueAsync(new GetSecretValueRequest
        {
            SecretId = "Messenger/SNS"
        });
        
        return JsonConvert.DeserializeObject<SNSConfiguration>(response.SecretString);
    }
}