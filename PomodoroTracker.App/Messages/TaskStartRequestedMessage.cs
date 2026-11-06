// Autor: Tým / Spoločné

namespace PomodoroTracker.App.Messages;

public record TaskStartRequestedMessage
{
    public required Guid TaskId { get; init; }
}
