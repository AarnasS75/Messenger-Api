namespace Message.Handlers.Contracts.Messages;

public class UserAuthenticatedEventMessage
{
    public string PhoneNumber { get; set; }
    public string Payload { get; set; }
}