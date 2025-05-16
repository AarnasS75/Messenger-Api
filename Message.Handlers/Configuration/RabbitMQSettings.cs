namespace Message.Handlers.Configuration;

public class RabbitMQSettings
{
    public string Host { get; set; }
    public ushort Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public List<QueueSettings> Queues { get; set; }
}

public class QueueSettings
{
    public string QueueName { get; set; }
    public string Exchange { get; set; }
    public string RoutingKey { get; set; }
    public string ExchangeType { get; set; }
    public string Consumer { get; set; }
}