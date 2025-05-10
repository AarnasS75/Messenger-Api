using Messenger.Infrastructure.Configuration;

namespace Messenger.Infrastructure.Interfaces;

public interface IVonageConfigurationProvider
{
    Task<VonageConfiguration> GetConfigurationAsync();
}