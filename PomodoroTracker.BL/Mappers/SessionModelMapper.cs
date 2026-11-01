// Autor: xpeterm01

using PomodoroTracker.BL.Calculations;
using PomodoroTracker.BL.Models;
using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.BL.Mappers;

public class SessionModelMapper : ModelMapperBase<SessionEntity, SessionListModel, SessionDetailModel>
{
    public override SessionListModel MapToListModel(SessionEntity? entity)
        => entity is null
            ? SessionListModel.Empty
            : new SessionListModel
            {
                Id = entity.Id,
                Type = entity.Type,
                StartedAt = entity.StartedAt,
                EndedAt = entity.EndedAt,
                PlannedDuration = entity.PlannedDuration,
                ActualDuration = SessionCalculations.ActualDuration(entity),
                Overrun = SessionCalculations.Overrun(entity),
                TaskId = entity.TaskId,
                TaskName = entity.Task?.Name
            };

    public override SessionDetailModel MapToDetailModel(SessionEntity? entity)
        => entity is null
            ? SessionDetailModel.Empty
            : new SessionDetailModel
            {
                Id = entity.Id,
                Type = entity.Type,
                StartedAt = entity.StartedAt,
                EndedAt = entity.EndedAt,
                PlannedDuration = entity.PlannedDuration,
                PausedDuration = entity.PausedDuration,
                TaskId = entity.TaskId
            };

    public override SessionEntity MapToEntity(SessionDetailModel model)
        => new()
        {
            Id = model.Id,
            Type = model.Type,
            StartedAt = model.StartedAt,
            EndedAt = model.EndedAt,
            PlannedDuration = model.PlannedDuration,
            PausedDuration = model.PausedDuration,
            TaskId = model.TaskId
        };
}
