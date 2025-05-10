using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Messenger.Infrastructure.Configuration;
using Messenger.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace Messenger.Infrastructure.Services;

public class SnsConfigurationProvider : ISNSConfigurationProvider
{
    private readonly IAmazonSecretsManager _secretsManager;

    public SnsConfigurationProvider(IAmazonSecretsManager secretsManager)
    {
        _secretsManager = secretsManager;
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
            Console.WriteLine("Secret 'Messenger/SNS' not found. Returning default configuration.");
            return new SNSConfiguration();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error retrieving secret: {e}");
            throw;
        }
    }
}