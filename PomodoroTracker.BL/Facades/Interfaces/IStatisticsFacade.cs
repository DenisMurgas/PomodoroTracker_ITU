// Autor: xmurgad00

using PomodoroTracker.BL.Models.Statistics;

namespace PomodoroTracker.BL.Facades;

public interface IStatisticsFacade
{
    Task<StatisticsSummaryModel> GetSummaryAsync(DateTime from, DateTime to);
    Task<IReadOnlyList<TimelineItemModel>> GetTimelineAsync(DateTime day);
    Task<IReadOnlyList<ProjectTimeModel>> GetTimeByProjectAsync(DateTime from, DateTime to);
    Task<IReadOnlyList<DayTotalModel>> GetDailyTotalsAsync(DateTime from, DateTime to);
}
