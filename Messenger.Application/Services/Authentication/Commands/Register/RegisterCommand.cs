using MediatR;
using Messenger.Application.Services.Authentication.Common;

namespace Messenger.Application.Services.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName, 
    string LastName, 
    string Password, 
    string Email) : IRequest<AuthenticationResult>;