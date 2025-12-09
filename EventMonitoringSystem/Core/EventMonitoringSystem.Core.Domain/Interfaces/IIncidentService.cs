using EventMonitoringSystem.Core.Domain.Entities;

namespace EventMonitoringSystem.Core.Domain.interfaces;

public interface IIncidentService
{
    Task ProcessAsync(Event dto);
    Task<IReadOnlyCollection<Incident>> GetAllIncidentsAsync();
}