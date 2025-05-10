using Messenger.Domain.Enums;

namespace Messenger.Application.Configuration;

public class ChannelSettings
{
    public NotificationType Type { get; set; }
    public bool Enabled { get; set; }
}