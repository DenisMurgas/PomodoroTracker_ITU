// Autor: xpeterm01

using Microsoft.EntityFrameworkCore;
using PomodoroTracker.BL.Mappers;
using PomodoroTracker.BL.Models;
using PomodoroTracker.DAL.Entities;
using PomodoroTracker.DAL.Mappers;
using PomodoroTracker.DAL.UnitOfWork;

namespace PomodoroTracker.BL.Facades;

public class TimerProfileFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    TimerProfileModelMapper modelMapper)
    : FacadeBase<TimerProfileEntity, TimerProfileModel, TimerProfileModel, TimerProfileEntityMapper>(unitOfWorkFactory, modelMapper),
        ITimerProfileFacade
{
    public override async Task<IEnumerable<TimerProfileModel>> GetAsync()
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<TimerProfileEntity> entities = await uow
            .GetRepository<TimerProfileEntity, TimerProfileEntityMapper>()
            .Get()
            .ToListAsync()
            .ConfigureAwait(false);

        return ModelMapper.MapToListModel(entities.OrderBy(p => p.WorkDuration)).ToList();
    }
}
