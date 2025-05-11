namespace Messenger.Domain.Interfaces;

public interface ISmsProvider
{
    Task SendSmsAsync(string fromPhoneNumber, string toPhoneNumber, string message);
}