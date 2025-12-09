using AutoMapper;
using EventMonitoringSystem.Core.Domain.Entities;
using EventMonitoringSystem.Core.Domain.Interfaces;
using EventMonitoringSystem.Data.Domain.Entities;
using EventMonitoringSystem.Data.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventMonitoringSystem.Data.Infrastructure.Repositories;

public class IncidentRepository : IIncidentRepository
{
    private readonly MonitoringDbContext _dbContext;
    private readonly IMapper _mapper;

    public IncidentRepository(MonitoringDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task DeleteAsync(Guid id)
    {
        var dao = await _dbContext.Incidents.SingleOrDefaultAsync(x => x.Id == id);

        if (dao is null)
            return;

        _dbContext.Incidents.Remove(dao);
        await SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<Incident>> GetAllAsync()
    {
        var daos = await _dbContext.Incidents
            .Include(x => x.Events)
            .ToListAsync();

        return _mapper.Map<IReadOnlyCollection<Incident>>(daos);
    }

    public async Task<Incident> GetByIdAsync(Guid id)
    {
        var dao = await _dbContext.Incidents
               .Include(x => x.Events)
               .SingleOrDefaultAsync(x => x.Id == id);

        return dao is null ? null : _mapper.Map<Incident>(dao);
    }

    private async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, Incident incident)
    {
        var dao = await _dbContext.Incidents
        .Include(x => x.Events)
        .SingleOrDefaultAsync(x => x.Id == id);

        if (dao is null)
            return;

        _mapper.Map(incident, dao);

        await SaveChangesAsync();
    }

    public async Task AddAsync(Incident incident)
    {
        var dao = _mapper.Map<IncidentDao>(incident);
        _dbContext.Incidents.Add(dao);
        await SaveChangesAsync();
    }
}
