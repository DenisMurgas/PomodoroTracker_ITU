// Autor: xmurgad00

using Microsoft.EntityFrameworkCore;
using PomodoroTracker.BL.Calculations;
using PomodoroTracker.BL.Models.Statistics;
using PomodoroTracker.DAL.Entities;
using PomodoroTracker.DAL.Mappers;
using PomodoroTracker.DAL.UnitOfWork;

namespace PomodoroTracker.BL.Facades;

public class StatisticsFacade(IUnitOfWorkFactory unitOfWorkFactory) : IStatisticsFacade
{
    public async Task<StatisticsSummaryModel> GetSummaryAsync(DateTime from, DateTime to)
    {
        await using IUnitOfWork uow = unitOfWorkFactory.Create();
        List<SessionEntity> sessions = await LoadSessionsAsync(uow, from, to).ConfigureAwait(false);

        int completedTasks = await uow
            .GetRepository<TaskEntity, TaskEntityMapper>()
            .Get()
            .CountAsync(t => t.IsCompleted && t.CompletedAt >= from && t.CompletedAt < to)
            .ConfigureAwait(false);

        return StatisticsCalculator.Summary(sessions, completedTasks);
    }

    public async Task<IReadOnlyList<TimelineItemModel>> GetTimelineAsync(DateTime day)
    {
        await using IUnitOfWork uow = unitOfWorkFactory.Create();
        List<SessionEntity> sessions = await LoadSessionsAsync(uow, day.Date, day.Date.AddDays(1)).ConfigureAwait(false);

        return StatisticsCalculator.Timeline(sessions);
    }

    public async Task<IReadOnlyList<ProjectTimeModel>> GetTimeByProjectAsync(DateTime from, DateTime to)
    {
        await using IUnitOfWork uow = unitOfWorkFactory.Create();
        List<SessionEntity> sessions = await LoadSessionsAsync(uow, from, to).ConfigureAwait(false);

        return StatisticsCalculator.ByProject(sessions);
    }

    public async Task<IReadOnlyList<DayTotalModel>> GetDailyTotalsAsync(DateTime from, DateTime to)
    {
        await using IUnitOfWork uow = unitOfWorkFactory.Create();
        List<SessionEntity> sessions = await LoadSessionsAsync(uow, from, to).ConfigureAwait(false);

        return StatisticsCalculator.DailyTotals(sessions, from, to);
    }

    private static Task<List<SessionEntity>> LoadSessionsAsync(IUnitOfWork uow, DateTime from, DateTime to)
        => uow.GetRepository<SessionEntity, SessionEntityMapper>()
            .Get()
            .Include(s => s.Task)
            .ThenInclude(t => t!.Project)
            .Where(s => s.StartedAt >= from && s.StartedAt < to && s.EndedAt != null)
            .OrderBy(s => s.StartedAt)
            .ToListAsync();
}
