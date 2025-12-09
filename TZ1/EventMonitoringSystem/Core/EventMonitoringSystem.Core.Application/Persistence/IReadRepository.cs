using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace EventMonitoringSystem.Core.Application.Persistence;

public interface IReadRepository<TReadContext> where TReadContext : DbContext
{
    Task<TEntity?> GetByIdAsync<TEntity>(Guid id) where TEntity : class;
    Task<IEnumerable<TEntity>> GetAllAsync<TEntity>() where TEntity : class;
    Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
    TReadContext DbContext { get; }
}