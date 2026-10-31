// Autor: Tým / Spoločné

namespace PomodoroTracker.DAL.Entities;

public record TimerProfileEntity : IEntity
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required TimeSpan WorkDuration { get; set; }

    public required TimeSpan ShortBreakDuration { get; set; }

    public required TimeSpan LongBreakDuration { get; set; }

    public required int LongBreakInterval { get; set; }
}
