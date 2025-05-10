using Messenger.Application.Interfaces;
using Messenger.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.API.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    
    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
    {
        try
        {
            await _notificationService.SendNotificationAsync(request);
            return Ok("Notification sent successfully!");
        }
        catch (Exception exception)
        {
            return BadRequest($"Error sending notification: {exception}");
        }
    }
}