namespace Messenger.Infrastructure.Configuration;

public class SNSConfiguration
{
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string TopicArn { get; set; }
}