namespace Messenger.Domain.Interfaces;

public interface IVonageProvider
{
    Task SendSmsAsync(string fromPhoneNumber, string toPhoneNumber, string message);
}