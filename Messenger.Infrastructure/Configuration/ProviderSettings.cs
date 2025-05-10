namespace Messenger.Infrastructure.Configuration;

public class ProviderSettings
{
    public string Type { get; set; }
    public bool Enabled { get; set; }
    public int Priority { get; set; }
}