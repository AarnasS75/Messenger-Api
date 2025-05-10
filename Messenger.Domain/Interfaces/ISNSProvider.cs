namespace Messenger.Domain.Interfaces;

public interface ISNSProvider
{
    Task SendEmailAsync(string recipient, string subject, string message);
    Task SendSmsAsync(string phoneNumber, string message);
}