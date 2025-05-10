using Messenger.Domain.Enums;

namespace Messenger.Application.Configuration;

public class ProviderSettings
{
    public ProviderType Type { get; set; }
    public bool Enabled { get; set; }
    public int Priority { get; set; }
}