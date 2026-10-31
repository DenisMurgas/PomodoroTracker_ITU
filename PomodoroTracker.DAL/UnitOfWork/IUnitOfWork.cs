// Autor: xmurgad00

using PomodoroTracker.DAL.Entities;
using PomodoroTracker.DAL.Mappers;
using PomodoroTracker.DAL.Repositories;

namespace PomodoroTracker.DAL.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
        where TEntity : class, IEntity
        where TEntityMapper : class, IEntityMapper<TEntity>;

    Task CommitAsync(CancellationToken cancellationToken = default);
}
