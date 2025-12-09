using EventMonitoringSystem.Core.Domain.Entities;

namespace EventMonitoringSystem.Core.Domain.Interfaces;

public interface IIncidentRepository
{
    Task<Incident> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<Incident>> GetAllAsync();
    Task UpdateAsync(Guid Id, Incident incident);
    Task DeleteAsync(Guid Id);
    Task AddAsync(Incident incident);
}
