// Autor: xpeterm01

using Microsoft.EntityFrameworkCore;
using PomodoroTracker.BL.Mappers;
using PomodoroTracker.BL.Models;
using PomodoroTracker.DAL.Entities;
using PomodoroTracker.DAL.Mappers;
using PomodoroTracker.DAL.UnitOfWork;

namespace PomodoroTracker.BL.Facades;

public class SessionFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    SessionModelMapper modelMapper)
    : FacadeBase<SessionEntity, SessionListModel, SessionDetailModel, SessionEntityMapper>(unitOfWorkFactory, modelMapper),
        ISessionFacade
{
    public async Task<IEnumerable<SessionListModel>> GetByRangeAsync(DateTime from, DateTime to)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<SessionEntity> entities = await uow
            .GetRepository<SessionEntity, SessionEntityMapper>()
            .Get()
            .Include(s => s.Task)
            .Where(s => s.StartedAt >= from && s.StartedAt < to)
            .OrderBy(s => s.StartedAt)
            .ToListAsync()
            .ConfigureAwait(false);

        return ModelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<SessionDetailModel>> GetUnfinishedAsync()
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<SessionEntity> entities = await uow
            .GetRepository<SessionEntity, SessionEntityMapper>()
            .Get()
            .Where(s => s.EndedAt == null)
            .ToListAsync()
            .ConfigureAwait(false);

        return entities.Select(ModelMapper.MapToDetailModel).ToList();
    }
}
