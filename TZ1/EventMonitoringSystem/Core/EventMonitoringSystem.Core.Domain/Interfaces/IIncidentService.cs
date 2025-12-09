using EventMonitoringSystem.Core.Domain.Dto_s;

namespace EventMonitoringSystem.Core.Domain.interfaces;

public interface IIncidentService
{
    Task ProcessAsync(EventDto dto);
    Task<List<IncidentDto>> GetAllIncidentsAsync();
}