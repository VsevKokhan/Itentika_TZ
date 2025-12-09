using EventMonitoringSystem.Core.Application.Models.Responses;
using EventMonitoringSystem.Core.Domain.Dto_s;
using EventMonitoringSystem.Core.Domain.interfaces;
using Microsoft.AspNetCore.Mvc;
using IResult = EventMonitoringSystem.Core.Application.Models.Responses.IResult;

namespace EventMonitoringSystem.Processor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IIncidentService _service;
    private readonly ILogger<EventsController> _logger;

    public EventsController(IIncidentService service, ILogger<EventsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IResult> Receive([FromBody] EventDto dto)
    {
        try
        {
            await _service.ProcessAsync(dto);
            return await Result.SuccessAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка во время отправки ивента");
            return await Result.FailAsync();
        }
    }
}