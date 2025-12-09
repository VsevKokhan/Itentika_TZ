using EventMonitoringSystem.Core.Application.Persistence;
using EventMonitoringSystem.DB.Main.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventMonitoringSystem.DB.Main.Infrastructure.Repositories;

public class WriteRepository : IWriteRepository<MonitoringDbContext>
{
    private readonly MonitoringDbContext _context;

    public WriteRepository(MonitoringDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }

    public async Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        await _context.Set<TEntity>().AddRangeAsync(entities);
    }

    public bool Update<TEntity>(TEntity entity) where TEntity : class
    {
        var exists = _context.Set<TEntity>().Any(e => e == entity);
        if (!exists) 
            return false;

        _context.Set<TEntity>().Update(entity);
        return true;
    }

    public bool Remove<TEntity>(TEntity entity) where TEntity : class
    {
        var exists = _context.Set<TEntity>().Any(e => e == entity);
        if (!exists) 
            return false;

        _context.Set<TEntity>().Remove(entity);
        
        return true;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public MonitoringDbContext DbContext { get => _context; }
}