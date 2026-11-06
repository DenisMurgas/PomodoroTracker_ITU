// Autor: xmurgad00

using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using PomodoroTracker.App.Messages;
using PomodoroTracker.App.Models;
using PomodoroTracker.App.Services;
using PomodoroTracker.BL.Calculations;
using PomodoroTracker.BL.Facades;
using PomodoroTracker.BL.Models;
using PomodoroTracker.BL.Models.Statistics;
using PomodoroTracker.Common.Enums;

namespace PomodoroTracker.App.ViewModels;

public partial class StatisticsViewModel(
    IStatisticsFacade statisticsFacade,
    IProjectFacade projectFacade,
    TimeProvider timeProvider,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<SessionFinishedMessage>
{
    private const int DefaultStartHour = 8;
    private const int DefaultEndHour = 17;
    private const int TickCount = 4;

    private static readonly string[] DayLabels = ["Po", "Ut", "St", "Št", "Pi", "So", "Ne"];
    private static readonly CultureInfo SlovakCulture = CultureInfo.GetCultureInfo("sk-SK");

    [ObservableProperty]
    public partial StatisticsSummaryModel Summary { get; set; } = new(TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, 0, 0);

    [ObservableProperty]
    public partial IReadOnlyList<TimelineSegmentModel> TimelineSegments { get; set; } = [];

    [ObservableProperty]
    public partial string TimelineStartText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string TimelineFirstThirdText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string TimelineSecondThirdText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string TimelineEndText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial IReadOnlyList<ProjectStatRowModel> Projects { get; set; } = [];

    [ObservableProperty]
    public partial IReadOnlyList<WeekDayModel> WeekDays { get; set; } = [];

    [ObservableProperty]
    public partial TimeSpan WeekTotal { get; set; }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        await ReloadAsync();

        ForceDataRefreshOnNextAppearing();
    }

    public async void Receive(SessionFinishedMessage message)
    {
        await ReloadAsync();
    }

    private async Task ReloadAsync()
    {
        DateTime now = timeProvider.GetLocalNow().DateTime;
        (DateTime dayFrom, DateTime dayTo) = StatisticsCalculator.GetRange(StatisticsPeriod.Day, now);
        (DateTime weekFrom, DateTime weekTo) = StatisticsCalculator.GetRange(StatisticsPeriod.Week, now);

        Summary = await statisticsFacade.GetSummaryAsync(dayFrom, dayTo);
        BuildTimeline(dayFrom, await statisticsFacade.GetTimelineAsync(dayFrom));
        BuildProjects(await statisticsFacade.GetTimeByProjectAsync(dayFrom, dayTo), await projectFacade.GetAsync());
        BuildWeek(await statisticsFacade.GetDailyTotalsAsync(weekFrom, weekTo), DateOnly.FromDateTime(now));
    }

    private void BuildTimeline(DateTime day, IReadOnlyList<TimelineItemModel> items)
    {
        int startHour = DefaultStartHour;
        int endHour = DefaultEndHour;

        if (items.Count > 0)
        {
            DateTime lastEnd = items.Max(i => i.EndedAt);
            int lastHour = (int)Math.Ceiling((lastEnd - day).TotalHours);

            startHour = Math.Min(startHour, items.Min(i => i.StartedAt.Hour));
            endHour = Math.Max(endHour, Math.Min(lastHour, 24));
        }

        int step = (int)Math.Ceiling((endHour - startHour) / (double)(TickCount - 1));
        endHour = startHour + step * (TickCount - 1);
        if (endHour > 24)
        {
            startHour = Math.Max(0, startHour - (endHour - 24));
            endHour = startHour + step * (TickCount - 1);
        }

        DateTime rangeStart = day.AddHours(startHour);
        double rangeHours = endHour - startHour;

        double ToFraction(DateTime time) => Math.Clamp((time - rangeStart).TotalHours / rangeHours, 0, 1);

        List<TimelineSegmentModel> segments = [];
        foreach (TimelineItemModel item in items)
        {
            if (item.Type == SessionType.Work)
            {
                segments.Add(new TimelineSegmentModel(ToFraction(item.StartedAt), ToFraction(item.EndedAt), TimelineSegmentKind.Work));
                continue;
            }

            DateTime breakEnd = item.EndedAt - item.Overrun;
            segments.Add(new TimelineSegmentModel(ToFraction(item.StartedAt), ToFraction(breakEnd), TimelineSegmentKind.Break));

            if (item.Overrun > TimeSpan.Zero)
            {
                segments.Add(new TimelineSegmentModel(ToFraction(breakEnd), ToFraction(item.EndedAt), TimelineSegmentKind.Overrun));
            }
        }

        TimelineSegments = segments;
        TimelineStartText = $"{startHour}:00";
        TimelineFirstThirdText = $"{startHour + step}:00";
        TimelineSecondThirdText = $"{startHour + 2 * step}:00";
        TimelineEndText = $"{endHour}:00";
    }

    private void BuildProjects(IReadOnlyList<ProjectTimeModel> times, IEnumerable<ProjectListModel> projects)
    {
        List<ProjectTimeModel> rows = times.ToList();
        rows.AddRange(projects
            .Where(p => rows.All(r => r.ProjectId != p.Id))
            .Select(p => new ProjectTimeModel(p.Id, p.Name, p.Color, TimeSpan.Zero)));

        TimeSpan max = rows.Count == 0 ? TimeSpan.Zero : rows.Max(r => r.WorkTime);

        Projects = rows
            .OrderByDescending(r => r.WorkTime)
            .ThenBy(r => r.Name)
            .Select(r => new ProjectStatRowModel(r.Name, r.Color, r.WorkTime, max > TimeSpan.Zero ? r.WorkTime / max : 0))
            .ToList();
    }

    private void BuildWeek(IReadOnlyList<DayTotalModel> totals, DateOnly today)
    {
        TimeSpan max = totals.Count == 0 ? TimeSpan.Zero : totals.Max(t => t.WorkTime);

        WeekTotal = totals.Aggregate(TimeSpan.Zero, (sum, t) => sum + t.WorkTime);
        WeekDays = totals
            .Select((t, index) => new WeekDayModel(
                index,
                DayLabels[index % DayLabels.Length],
                max > TimeSpan.Zero ? t.WorkTime / max : 0,
                t.Day == today,
                t.Day == today ? t.WorkTime.TotalHours.ToString("0.0", SlovakCulture) + " h" : string.Empty))
            .ToList();
    }
}
