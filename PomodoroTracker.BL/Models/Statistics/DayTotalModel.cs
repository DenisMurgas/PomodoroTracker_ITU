// Autor: xmurgad00

namespace PomodoroTracker.BL.Models.Statistics;

public record DayTotalModel(
    DateOnly Day,
    TimeSpan WorkTime);
