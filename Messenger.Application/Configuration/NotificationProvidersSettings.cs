namespace Messenger.Application.Configuration;

public class NotificationProvidersSettings
{
    public const string SectionName = "NotificationsConfiguration";
    public List<ProviderSettings> Providers { get; set; }
    public List<ChannelSettings> Channels { get; set; }
}