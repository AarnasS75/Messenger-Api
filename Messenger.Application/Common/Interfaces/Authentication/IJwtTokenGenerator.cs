using Messenger.Domain.Entities;

namespace Messenger.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateJwtToken(UserEntity user);
}