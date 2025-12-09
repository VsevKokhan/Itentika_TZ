using Microsoft.EntityFrameworkCore;

namespace EventMonitoringSystem.Core.Application.Persistence;

public interface IWriteRepository<TWriteContext> where TWriteContext : DbContext
{
    Task AddAsync<TEntity>(TEntity entity) where TEntity : class;
    Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
    bool Update<TEntity>(TEntity entity) where TEntity : class;
    bool Remove<TEntity>(TEntity entity) where TEntity : class;
    Task SaveChangesAsync();
    TWriteContext DbContext { get; }
}