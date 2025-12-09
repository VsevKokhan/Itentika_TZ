using AutoMapper;
using EventMonitoringSystem.Core.Domain.Entities;
using EventMonitoringSystem.Core.Domain.Interfaces;
using EventMonitoringSystem.Data.Domain.Entities;
using EventMonitoringSystem.Data.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventMonitoringSystem.Data.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly MonitoringDbContext _dbContext;
    private readonly IMapper _mapper;

    public EventRepository(MonitoringDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task AddAsync(Event ev)
    {
        var dao = _mapper.Map<EventDao>(ev);
        _dbContext.Events.Add(dao);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var dao = await _dbContext.Events.SingleOrDefaultAsync(x => x.Id == id);

        if (dao is null)
            return;

        _dbContext.Events.Remove(dao);
        await SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<Event>> GetAllAsync()
    {
        var daos = await _dbContext.Events.ToListAsync();
        return _mapper.Map<IReadOnlyCollection<Event>>(daos);
    }

    public async Task<Event> GetByIdAsync(Guid id)
    {
        var dao = await _dbContext.Events
            .SingleOrDefaultAsync(x => x.Id == id);

        return dao is null ? null : _mapper.Map<Event>(dao);
    }

    public async Task UpdateAsync(Guid id, Event @event)
    {
        var dao = await _dbContext.Events
            .SingleOrDefaultAsync(x => x.Id == id);

        if (dao is null)
            return;

        _mapper.Map(@event, dao);

        await SaveChangesAsync();
    }

    private async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
