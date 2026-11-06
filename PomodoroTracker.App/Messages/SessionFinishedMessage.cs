// Autor: Tým / Spoločné

using PomodoroTracker.BL.Models;

namespace PomodoroTracker.App.Messages;

public record SessionFinishedMessage
{
    public required SessionDetailModel Session { get; init; }
}
