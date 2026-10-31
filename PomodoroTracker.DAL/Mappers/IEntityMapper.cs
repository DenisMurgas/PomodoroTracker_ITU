// Autor: xmurgad00

using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.DAL.Mappers;

public interface IEntityMapper<in TEntity>
    where TEntity : IEntity
{
    void MapToExistingEntity(TEntity existingEntity, TEntity newEntity);
}
