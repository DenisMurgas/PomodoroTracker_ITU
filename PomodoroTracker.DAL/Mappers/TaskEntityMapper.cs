// Autor: xmurgad00

using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.DAL.Mappers;

public class TaskEntityMapper : IEntityMapper<TaskEntity>
{
    public void MapToExistingEntity(TaskEntity existingEntity, TaskEntity newEntity)
    {
        existingEntity.Name = newEntity.Name;
        existingEntity.IsCompleted = newEntity.IsCompleted;
        existingEntity.CreatedAt = newEntity.CreatedAt;
        existingEntity.CompletedAt = newEntity.CompletedAt;
        existingEntity.ProjectId = newEntity.ProjectId;
    }
}
