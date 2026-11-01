// Autor: xpeterm01

using PomodoroTracker.BL.Models;
using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.BL.Mappers;

public class SettingsModelMapper : ModelMapperBase<SettingsEntity, SettingsModel, SettingsModel>
{
    public override SettingsModel MapToListModel(SettingsEntity? entity)
        => MapToDetailModel(entity);

    public override SettingsModel MapToDetailModel(SettingsEntity? entity)
        => entity is null
            ? SettingsModel.Empty
            : new SettingsModel
            {
                Id = entity.Id,
                AutoStartNext = entity.AutoStartNext,
                NotificationsEnabled = entity.NotificationsEnabled,
                ActiveTimerProfileId = entity.ActiveTimerProfileId
            };

    public override SettingsEntity MapToEntity(SettingsModel model)
        => new()
        {
            Id = model.Id,
            AutoStartNext = model.AutoStartNext,
            NotificationsEnabled = model.NotificationsEnabled,
            ActiveTimerProfileId = model.ActiveTimerProfileId
        };
}
