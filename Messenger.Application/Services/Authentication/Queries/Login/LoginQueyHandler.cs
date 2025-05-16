using MediatR;
using Messenger.Application.Common.Interfaces.Authentication;
using Messenger.Application.Common.Interfaces.Persistence;
using Messenger.Application.Services.Authentication.Common;

namespace Messenger.Application.Services.Authentication.Queries.Login;

public class LoginQueyHandler : IRequestHandler<LoginQuery, AuthenticationResult>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public LoginQueyHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<AuthenticationResult> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
        {
            throw new Exception("Invalid email or password");
        }

        if (user.Password != request.Password)
        {
            throw new Exception("Invalid password");
        }

        var token = _jwtTokenGenerator.GenerateJwtToken(user);

        return new AuthenticationResult(user, token);
    }
}