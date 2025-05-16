namespace Messenger.Domain.Entities;

public class UserEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string FirstName { get; set; }   
    public required string LastName { get; set; }   
    public required string Password { get; set; }   
    public required string Email { get; set; }   
}