// Autor: xmurgad00

using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.DAL.Mappers;

public class TimerProfileEntityMapper : IEntityMapper<TimerProfileEntity>
{
    public void MapToExistingEntity(TimerProfileEntity existingEntity, TimerProfileEntity newEntity)
    {
        existingEntity.Name = newEntity.Name;
        existingEntity.WorkDuration = newEntity.WorkDuration;
        existingEntity.ShortBreakDuration = newEntity.ShortBreakDuration;
        existingEntity.LongBreakDuration = newEntity.LongBreakDuration;
        existingEntity.LongBreakInterval = newEntity.LongBreakInterval;
    }
}
