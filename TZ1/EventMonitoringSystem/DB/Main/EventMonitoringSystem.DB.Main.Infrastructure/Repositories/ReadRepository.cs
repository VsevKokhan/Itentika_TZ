using System.Linq.Expressions;
using EventMonitoringSystem.Core.Application.Persistence;
using EventMonitoringSystem.DB.Main.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventMonitoringSystem.DB.Main.Infrastructure.Repositories;

public class ReadRepository : IReadRepository<MonitoringDbContext> 
{
    private readonly MonitoringDbContext _context;

    public ReadRepository(MonitoringDbContext context)
    {
        _context = context;
    }

    
    public async Task<TEntity?> GetByIdAsync<TEntity>(Guid id) where TEntity : class
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>() where TEntity : class
    {
        return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
    {
        return await _context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync();
    }

    public MonitoringDbContext DbContext { get => _context; }
}