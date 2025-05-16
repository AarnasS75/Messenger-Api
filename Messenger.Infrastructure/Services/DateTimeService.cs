using Messenger.Application.Common.Interfaces.Providers;

namespace Messenger.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.Now;
}