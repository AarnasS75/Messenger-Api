namespace Message.Handlers.Messages;

public class ShipmentCompletedEventMesssage
{
    public string ShipmentId { get; set; }
    public string Recipient { get; set; }
    public string Body { get; set; }
}