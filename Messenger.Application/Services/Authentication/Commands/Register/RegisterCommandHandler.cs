using MediatR;
using Messenger.Application.Common.Interfaces.Authentication;
using Messenger.Application.Common.Interfaces.Persistence;
using Messenger.Application.Services.Authentication.Common;
using Messenger.Contracts.Authentication;
using Messenger.Domain.Entities;

namespace Messenger.Application.Services.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterCommandHandler(
        IUserRepository userRepository, 
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user != null)
        {
            throw new Exception($"Email {request.Email} is already registered.");
        }

        user = new UserEntity
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Password = request.Password,
            Email = request.Email
        };
        await _userRepository.Insert(user);
        
        var token = _jwtTokenGenerator.GenerateJwtToken(user);

        return new AuthenticationResult(user, token);
    }
}