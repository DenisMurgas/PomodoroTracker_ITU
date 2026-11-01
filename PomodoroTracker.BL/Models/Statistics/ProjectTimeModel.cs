// Autor: xmurgad00

namespace PomodoroTracker.BL.Models.Statistics;

public record ProjectTimeModel(
    Guid? ProjectId,
    string Name,
    string Color,
    TimeSpan WorkTime);
