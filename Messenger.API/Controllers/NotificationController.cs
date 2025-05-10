using Messenger.Application.Interfaces;
using Messenger.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.API.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly ISmsService _smsService;

    public NotificationController(IEmailService emailService, ISmsService smsService)
    {
        _emailService = emailService;
        _smsService = smsService;
    }
    
    [HttpPost("sms")]
    public async Task<IActionResult> SendSms([FromBody] SmsNotificationRequest request)
    {
        try
        {
            await _smsService.SendAsync(request);
            return Ok("SMS sent successfully!");
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
            await _emailService.SendAsync(request);
            return Ok("Email sent successfully!");
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to send Email: {ex.Message}");
        }
    }
}