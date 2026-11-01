// Autor: xpeterm01

using PomodoroTracker.BL.Models;
using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.BL.Mappers;

public class TimerProfileModelMapper : ModelMapperBase<TimerProfileEntity, TimerProfileModel, TimerProfileModel>
{
    public override TimerProfileModel MapToListModel(TimerProfileEntity? entity)
        => MapToDetailModel(entity);

    public override TimerProfileModel MapToDetailModel(TimerProfileEntity? entity)
        => entity is null
            ? TimerProfileModel.Empty
            : new TimerProfileModel
            {
                Id = entity.Id,
                Name = entity.Name,
                WorkDuration = entity.WorkDuration,
                ShortBreakDuration = entity.ShortBreakDuration,
                LongBreakDuration = entity.LongBreakDuration,
                LongBreakInterval = entity.LongBreakInterval
            };

    public override TimerProfileEntity MapToEntity(TimerProfileModel model)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            WorkDuration = model.WorkDuration,
            ShortBreakDuration = model.ShortBreakDuration,
            LongBreakDuration = model.LongBreakDuration,
            LongBreakInterval = model.LongBreakInterval
        };
}
