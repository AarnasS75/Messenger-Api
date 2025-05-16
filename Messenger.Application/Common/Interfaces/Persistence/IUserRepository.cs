using Messenger.Domain.Entities;

namespace Messenger.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    Task Insert(UserEntity entity);
    Task<UserEntity?> GetByEmailAsync(string email);
}