// Autor: Tým / Spoločné

namespace PomodoroTracker.DAL.Entities;

public record SettingsEntity : IEntity
{
    public required Guid Id { get; set; }

    public bool AutoStartNext { get; set; } = true;

    public bool NotificationsEnabled { get; set; } = true;

    public Guid? ActiveTimerProfileId { get; set; }

    public TimerProfileEntity? ActiveTimerProfile { get; init; }
}
