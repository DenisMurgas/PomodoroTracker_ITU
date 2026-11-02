// Autor: xmurgad00

using PomodoroTracker.BL.Models.Statistics;
using PomodoroTracker.Common.Enums;
using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.BL.Calculations;

public static class StatisticsCalculator
{
    public const string NoProjectName = "Bez projektu";
    public const string NoProjectColor = "#9CA3AF";

    public static (DateTime From, DateTime To) GetRange(StatisticsPeriod period, DateTime reference)
    {
        DateTime day = reference.Date;

        return period switch
        {
            StatisticsPeriod.Day => (day, day.AddDays(1)),
            StatisticsPeriod.Week => WeekRange(day),
            StatisticsPeriod.Month => (new DateTime(day.Year, day.Month, 1), new DateTime(day.Year, day.Month, 1).AddMonths(1)),
            _ => throw new ArgumentOutOfRangeException(nameof(period), period, null)
        };
    }

    public static StatisticsSummaryModel Summary(IReadOnlyCollection<SessionEntity> sessions, int completedTasks)
    {
        List<SessionEntity> finished = sessions.Where(s => s.EndedAt is not null).ToList();
        List<SessionEntity> breaks = finished.Where(s => s.Type != SessionType.Work).ToList();

        TimeSpan workTime = SessionCalculations.WorkTime(finished);
        TimeSpan overrun = SessionCalculations.Sum(breaks.Select(SessionCalculations.Overrun));
        TimeSpan breakTime = SessionCalculations.Sum(breaks.Select(SessionCalculations.ActualDuration)) - overrun;
        int workBlocks = finished.Count(s => s.Type == SessionType.Work);

        return new StatisticsSummaryModel(workTime, breakTime, overrun, workBlocks, completedTasks);
    }

    public static IReadOnlyList<TimelineItemModel> Timeline(IEnumerable<SessionEntity> sessions)
        => sessions
            .Where(s => s.EndedAt is not null)
            .OrderBy(s => s.StartedAt)
            .Select(s => new TimelineItemModel(
                s.Id,
                s.Type,
                s.StartedAt,
                s.EndedAt!.Value,
                SessionCalculations.Overrun(s),
                s.Task?.Name,
                s.Task?.Project?.Color))
            .ToList();

    public static IReadOnlyList<ProjectTimeModel> ByProject(IEnumerable<SessionEntity> sessions)
        => sessions
            .Where(SessionCalculations.IsFinishedWork)
            .GroupBy(s => s.Task?.Project?.Id)
            .Select(group =>
            {
                ProjectEntity? project = group.First().Task?.Project;
                return new ProjectTimeModel(
                    project?.Id,
                    project?.Name ?? NoProjectName,
                    project?.Color ?? NoProjectColor,
                    SessionCalculations.Sum(group.Select(SessionCalculations.ActualDuration)));
            })
            .OrderByDescending(p => p.WorkTime)
            .ToList();

    public static IReadOnlyList<DayTotalModel> DailyTotals(IEnumerable<SessionEntity> sessions, DateTime from, DateTime to)
    {
        Dictionary<DateOnly, TimeSpan> totals = sessions
            .Where(SessionCalculations.IsFinishedWork)
            .GroupBy(s => DateOnly.FromDateTime(s.StartedAt))
            .ToDictionary(g => g.Key, g => SessionCalculations.Sum(g.Select(SessionCalculations.ActualDuration)));

        List<DayTotalModel> result = [];
        for (DateTime day = from.Date; day < to; day = day.AddDays(1))
        {
            DateOnly key = DateOnly.FromDateTime(day);
            result.Add(new DayTotalModel(key, totals.GetValueOrDefault(key, TimeSpan.Zero)));
        }

        return result;
    }

    private static (DateTime From, DateTime To) WeekRange(DateTime day)
    {
        int daysSinceMonday = ((int)day.DayOfWeek + 6) % 7;
        DateTime monday = day.AddDays(-daysSinceMonday);
        return (monday, monday.AddDays(7));
    }
}
