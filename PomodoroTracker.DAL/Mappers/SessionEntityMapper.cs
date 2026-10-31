// Autor: xmurgad00

using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.DAL.Mappers;

public class SessionEntityMapper : IEntityMapper<SessionEntity>
{
    public void MapToExistingEntity(SessionEntity existingEntity, SessionEntity newEntity)
    {
        existingEntity.Type = newEntity.Type;
        existingEntity.StartedAt = newEntity.StartedAt;
        existingEntity.EndedAt = newEntity.EndedAt;
        existingEntity.PlannedDuration = newEntity.PlannedDuration;
        existingEntity.PausedDuration = newEntity.PausedDuration;
        existingEntity.TaskId = newEntity.TaskId;
    }
}
