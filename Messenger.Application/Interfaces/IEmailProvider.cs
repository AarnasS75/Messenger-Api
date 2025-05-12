namespace Messenger.Application.Interfaces;

public interface IEmailProvider
{
    Task SendEmailAsync(string recipient, string subject, string message);
}