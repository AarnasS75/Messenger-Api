using Messenger.Application.Common.Interfaces.Persistence;
using Messenger.Domain.Entities;

namespace Messenger.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private static List<UserEntity> _users = [];
    
    public async Task Insert(UserEntity entity)
    {
        _users.Add(entity);
    }

    public async Task<UserEntity?> GetByEmailAsync(string email)
    {
        return _users.Find(u => u.Email == email);
    }
}