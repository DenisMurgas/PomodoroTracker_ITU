// Autor: xmurgad00

using Microsoft.EntityFrameworkCore;
using PomodoroTracker.BL.Mappers;
using PomodoroTracker.BL.Models;
using PomodoroTracker.Common.Enums;
using PomodoroTracker.DAL.Entities;
using PomodoroTracker.DAL.Mappers;
using PomodoroTracker.DAL.UnitOfWork;

namespace PomodoroTracker.BL.Facades;

public class TaskFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    TaskModelMapper modelMapper,
    TimeProvider timeProvider)
    : FacadeBase<TaskEntity, TaskListModel, TaskDetailModel, TaskEntityMapper>(unitOfWorkFactory, modelMapper),
        ITaskFacade
{
    public override async Task<IEnumerable<TaskListModel>> GetAsync()
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<TaskEntity> entities = await QueryWithDetails(uow)
            .ToListAsync()
            .ConfigureAwait(false);

        return Order(ModelMapper.MapToListModel(entities));
    }

    public async Task<IEnumerable<TaskListModel>> GetByProjectAsync(Guid projectId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<TaskEntity> entities = await QueryWithDetails(uow)
            .Where(t => t.ProjectId == projectId)
            .ToListAsync()
            .ConfigureAwait(false);

        return Order(ModelMapper.MapToListModel(entities));
    }

    public async Task<TaskListModel?> GetLastWorkedAsync()
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        Guid? lastTaskId = await uow
            .GetRepository<SessionEntity, SessionEntityMapper>()
            .Get()
            .Where(s => s.Type == SessionType.Work && s.TaskId != null)
            .OrderByDescending(s => s.StartedAt)
            .Select(s => s.TaskId)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

        if (lastTaskId is null)
        {
            return null;
        }

        TaskEntity? entity = await QueryWithDetails(uow)
            .SingleOrDefaultAsync(t => t.Id == lastTaskId)
            .ConfigureAwait(false);

        return entity is null ? null : ModelMapper.MapToListModel(entity);
    }

    public async Task SetCompletedAsync(Guid taskId, bool isCompleted)
    {
        TaskDetailModel task = await GetAsync(taskId).ConfigureAwait(false)
                               ?? throw new InvalidOperationException($"Task '{taskId}' was not found.");

        task.IsCompleted = isCompleted;
        await SaveAsync(task).ConfigureAwait(false);
    }

    public override Task<TaskDetailModel> SaveAsync(TaskDetailModel model)
    {
        DateTime now = timeProvider.GetLocalNow().DateTime;

        if (model.CreatedAt == default)
        {
            model.CreatedAt = now;
        }

        model.CompletedAt = model.IsCompleted ? model.CompletedAt ?? now : null;

        return base.SaveAsync(model);
    }

    private static IQueryable<TaskEntity> QueryWithDetails(IUnitOfWork uow)
        => uow.GetRepository<TaskEntity, TaskEntityMapper>()
            .Get()
            .Include(t => t.Project)
            .Include(t => t.Sessions);

    private static IEnumerable<TaskListModel> Order(IEnumerable<TaskListModel> tasks)
        => tasks
            .OrderBy(t => t.IsCompleted)
            .ThenByDescending(t => t.LastWorkedAt ?? DateTime.MinValue)
            .ThenBy(t => t.Name)
            .ToList();
}
