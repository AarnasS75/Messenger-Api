namespace Messenger.Infrastructure.Configuration;

public class NotificationProvidersSettings
{
    public const string SectionName = "NotificationsConfiguration";
    public List<ProviderSettings> Providers { get; set; }
}