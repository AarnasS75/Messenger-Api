using Messenger.Domain.Enums;

namespace Messenger.Application.Configuration;

public class ChannelConfiguration
{
    public bool Enabled { get; set; }
    public Dictionary<ProviderType, ProviderConfiguration> Providers { get; set; }
}