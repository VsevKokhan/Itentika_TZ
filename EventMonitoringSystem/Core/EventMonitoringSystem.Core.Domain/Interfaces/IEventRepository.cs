using EventMonitoringSystem.Core.Domain.Entities;

namespace EventMonitoringSystem.Core.Domain.Interfaces;

public interface IEventRepository
{
    Task<Event> GetByIdAsync(Guid id);
    Task<IReadOnlyCollection<Event>> GetAllAsync();
    Task UpdateAsync(Guid Id, Event incident);
    Task DeleteAsync(Guid Id);
    Task AddAsync(Event ev);
}
