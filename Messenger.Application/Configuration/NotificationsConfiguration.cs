namespace Messenger.Application.Configuration;

public class NotificationsConfiguration
{
    public const string SectionName = "NotificationsConfiguration";
    
    public ChannelConfiguration Email { get; set; }
    public ChannelConfiguration Sms { get; set; }
}