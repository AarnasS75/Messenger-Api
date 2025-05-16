namespace Messenger.Application.Common.Interfaces.Providers;

public interface IDateTimeService
{
    DateTime UtcNow { get; }
}