namespace Message.Handlers.Contracts.Messages;

public class ShipmentCompletedEventMesssage
{
    public string ShipmentId { get; set; }
    public string UserName { get; set; }
}