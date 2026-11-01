// Autor: xpeterm01

using Microsoft.EntityFrameworkCore;
using PomodoroTracker.BL.Mappers;
using PomodoroTracker.BL.Models;
using PomodoroTracker.DAL.Entities;
using PomodoroTracker.DAL.Mappers;
using PomodoroTracker.DAL.UnitOfWork;

namespace PomodoroTracker.BL.Facades;

public class SettingsFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    SettingsModelMapper modelMapper)
    : FacadeBase<SettingsEntity, SettingsModel, SettingsModel, SettingsEntityMapper>(unitOfWorkFactory, modelMapper),
        ISettingsFacade
{
    public async Task<SettingsModel> GetCurrentAsync()
    {
        await using (IUnitOfWork uow = UnitOfWorkFactory.Create())
        {
            SettingsEntity? entity = await uow
                .GetRepository<SettingsEntity, SettingsEntityMapper>()
                .Get()
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (entity is not null)
            {
                return ModelMapper.MapToDetailModel(entity);
            }
        }

        return await SaveAsync(SettingsModel.Empty).ConfigureAwait(false);
    }
}
