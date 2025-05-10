using Messenger.Infrastructure.Configuration;

namespace Messenger.Infrastructure.Interfaces;

public interface ISNSConfigurationProvider
{
    Task<SNSConfiguration> GetConfigurationAsync();
}