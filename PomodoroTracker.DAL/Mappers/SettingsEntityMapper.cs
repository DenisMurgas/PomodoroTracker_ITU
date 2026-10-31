// Autor: xmurgad00

using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.DAL.Mappers;

public class SettingsEntityMapper : IEntityMapper<SettingsEntity>
{
    public void MapToExistingEntity(SettingsEntity existingEntity, SettingsEntity newEntity)
    {
        existingEntity.AutoStartNext = newEntity.AutoStartNext;
        existingEntity.NotificationsEnabled = newEntity.NotificationsEnabled;
        existingEntity.ActiveTimerProfileId = newEntity.ActiveTimerProfileId;
    }
}
