// Autor: Tým / Spoločné

using PomodoroTracker.Common.Enums;

namespace PomodoroTracker.App.Messages;

public record PlannedTimeElapsedMessage
{
    public required SessionType Phase { get; init; }
}
