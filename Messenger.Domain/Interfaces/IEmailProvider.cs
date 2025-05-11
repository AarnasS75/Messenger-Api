namespace Messenger.Domain.Interfaces;

public interface IEmailProvider
{
    Task SendEmailAsync(string recipient, string subject, string message);
}