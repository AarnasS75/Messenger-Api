using Messenger.Domain.Entities;

namespace Messenger.Application.Services.Authentication.Common;

public record AuthenticationResult(UserEntity User, string Token);