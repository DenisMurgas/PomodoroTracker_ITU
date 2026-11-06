// Autor: Tým / Spoločné

namespace PomodoroTracker.App.Messages;

public record TaskChangedMessage
{
    public required Guid TaskId { get; init; }
}
