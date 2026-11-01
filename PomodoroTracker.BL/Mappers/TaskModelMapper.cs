// Autor: xmurgad00

using PomodoroTracker.BL.Calculations;
using PomodoroTracker.BL.Models;
using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.BL.Mappers;

public class TaskModelMapper : ModelMapperBase<TaskEntity, TaskListModel, TaskDetailModel>
{
    public override TaskListModel MapToListModel(TaskEntity? entity)
    {
        if (entity is null)
        {
            return TaskListModel.Empty;
        }

        List<SessionEntity> workSessions = entity.Sessions.Where(SessionCalculations.IsFinishedWork).ToList();

        return new TaskListModel
        {
            Id = entity.Id,
            Name = entity.Name,
            IsCompleted = entity.IsCompleted,
            ProjectId = entity.ProjectId,
            ProjectName = entity.Project?.Name ?? string.Empty,
            ProjectColor = entity.Project?.Color ?? string.Empty,
            TrackedTime = SessionCalculations.WorkTime(workSessions),
            WorkBlockCount = workSessions.Count,
            LastWorkedAt = workSessions.Count == 0 ? null : workSessions.Max(s => s.StartedAt)
        };
    }

    public override TaskDetailModel MapToDetailModel(TaskEntity? entity)
        => entity is null
            ? TaskDetailModel.Empty
            : new TaskDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                IsCompleted = entity.IsCompleted,
                CreatedAt = entity.CreatedAt,
                CompletedAt = entity.CompletedAt,
                ProjectId = entity.ProjectId
            };

    public override TaskEntity MapToEntity(TaskDetailModel model)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            IsCompleted = model.IsCompleted,
            CreatedAt = model.CreatedAt,
            CompletedAt = model.CompletedAt,
            ProjectId = model.ProjectId
        };
}
