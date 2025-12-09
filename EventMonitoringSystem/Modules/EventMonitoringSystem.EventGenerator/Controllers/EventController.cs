using EventMonitoringSystem.Core.Application.Models.Responses;
using EventMonitoringSystem.Core.Domain.Enums;
using EventMonitoringSystem.Core.Domain.interfaces;
using Microsoft.AspNetCore.Mvc;
using IResult = EventMonitoringSystem.Core.Application.Models.Responses.IResult;


namespace EventMonitoringSystem.EventGenerator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventPublisher _publisher;
    private readonly ILogger<EventController> _logger;

    public EventController(IEventPublisher publisher, ILogger<EventController> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IResult> SendEvent([FromBody] EventTypeEnum typeEnum)
    {
        try
        {
            await _publisher.PublishAsync(typeEnum);

            _logger.LogInformation("Manually sent event typeEnum {TypeEnum}", typeEnum);
            
            return await Result<EventTypeEnum>.SuccessAsync(typeEnum);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send event");
            
            return await Result<EventTypeEnum>.FailAsync();
        }
    }
}