namespace Messenger.Application.Interfaces;

public interface ISmsProvider
{
    Task SendSmsAsync(string fromPhoneNumber, string toPhoneNumber, string message);
}