namespace Messenger.Domain.Interfaces;

public interface ITwilioProvider
{
    Task SendSmsAsync(string fromPhoneNumber, string toPhoneNumber, string message);
}