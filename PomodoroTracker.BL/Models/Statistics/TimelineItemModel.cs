// Autor: xmurgad00

using PomodoroTracker.Common.Enums;

namespace PomodoroTracker.BL.Models.Statistics;

public record TimelineItemModel(
    Guid SessionId,
    SessionType Type,
    DateTime StartedAt,
    DateTime EndedAt,
    TimeSpan Overrun,
    string? TaskName,
    string? ProjectColor);
