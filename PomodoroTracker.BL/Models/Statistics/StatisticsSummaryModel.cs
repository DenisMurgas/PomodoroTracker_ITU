// Autor: xmurgad00

namespace PomodoroTracker.BL.Models.Statistics;

public record StatisticsSummaryModel(
    TimeSpan WorkTime,
    TimeSpan BreakTime,
    TimeSpan OverrunTime,
    int CompletedWorkBlocks,
    int CompletedTasks);
