using Messenger.Infrastructure.Configuration;

namespace Messenger.Infrastructure.Interfaces;

public interface ITwilioConfigurationProvider
{
    Task<TwilioConfiguration> GetConfigurationAsync();
}