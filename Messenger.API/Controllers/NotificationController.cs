using MapsterMapper;
using MediatR;
using Messenger.Application.Services.Notification.Commands.Email;
using Messenger.Application.Services.Notification.Commands.Sms;
using Messenger.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.API.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public NotificationController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("sms")]
    public async Task<IActionResult> SendSms([FromBody] SmsNotificationRequest request)
    {
        try
        {
            var command = _mapper.Map<SmsCommand>(request);
            await _mediator.Send(command);
            
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to send SMS: {ex.Message}");
        }
    }

    [HttpPost("email")]
    public async Task<IActionResult> SendEmail([FromBody] EmailNotificationRequest request)
    {
        try
        {
            var command = _mapper.Map<EmailCommand>(request);
            await _mediator.Send(command);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to send Email: {ex.Message}");
        }
    }
}