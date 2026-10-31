// Autor: xmurgad00

using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.DAL.Repositories;

public interface IRepository<TEntity>
    where TEntity : class, IEntity
{
    IQueryable<TEntity> Get();
    Task DeleteAsync(Guid entityId, CancellationToken cancellationToken = default);
    ValueTask<bool> ExistsAsync(TEntity entity, CancellationToken cancellationToken = default);
    TEntity Insert(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
}
