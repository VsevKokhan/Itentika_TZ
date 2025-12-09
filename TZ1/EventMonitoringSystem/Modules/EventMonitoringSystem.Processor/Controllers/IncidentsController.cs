using EventMonitoringSystem.Core.Application.Models.Responses;
using EventMonitoringSystem.Core.Domain.Dto_s;
using EventMonitoringSystem.Core.Domain.interfaces;
using Microsoft.AspNetCore.Mvc;
using IResult = EventMonitoringSystem.Core.Application.Models.Responses.IResult;

namespace EventMonitoringSystem.Processor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncidentsController : ControllerBase
{
    private readonly IIncidentService _incidentService;
    
    public IncidentsController(IIncidentService incidentService)
    {
        _incidentService = incidentService;
    }

    [HttpGet]
    public async Task<IResult> GetAll()
    {
        var incidents = await _incidentService.GetAllIncidentsAsync();
        return await Result<IncidentDto[]>.SuccessAsync(incidents.ToArray());
    }
}