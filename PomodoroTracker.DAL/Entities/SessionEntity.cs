// Autor: Tým / Spoločné

using PomodoroTracker.Common.Enums;

namespace PomodoroTracker.DAL.Entities;

public record SessionEntity : IEntity
{
    public required Guid Id { get; set; }

    public required SessionType Type { get; set; }

    public required DateTime StartedAt { get; set; }

    public DateTime? EndedAt { get; set; }

    public required TimeSpan PlannedDuration { get; set; }

    public TimeSpan PausedDuration { get; set; }

    public Guid? TaskId { get; set; }

    public TaskEntity? Task { get; init; }
}
