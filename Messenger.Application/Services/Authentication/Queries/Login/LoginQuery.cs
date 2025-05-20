using MediatR;
using Messenger.Application.Services.Authentication.Common;

namespace Messenger.Application.Services.Authentication.Queries.Login;

public record LoginQuery(string Email, string Password) : IRequest<AuthenticationResult>;