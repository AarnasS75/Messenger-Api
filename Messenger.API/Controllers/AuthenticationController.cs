using MapsterMapper;
using MediatR;
using Messenger.Application.Services.Authentication.Commands.Register;
using Messenger.Application.Services.Authentication.Queries.Login;
using Messenger.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    
    public AuthenticationController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        var response = await _mediator.Send(command);

        return Ok(response);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var command = _mapper.Map<LoginQuery>(request);
        var response = await _mediator.Send(command);
        
        return Ok(response);
    }
}