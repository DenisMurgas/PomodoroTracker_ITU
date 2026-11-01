// Autor: xmurgad00

using Microsoft.EntityFrameworkCore;
using PomodoroTracker.BL.Mappers;
using PomodoroTracker.BL.Models;
using PomodoroTracker.DAL.Entities;
using PomodoroTracker.DAL.Mappers;
using PomodoroTracker.DAL.UnitOfWork;

namespace PomodoroTracker.BL.Facades;

public class ProjectFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    ProjectModelMapper modelMapper)
    : FacadeBase<ProjectEntity, ProjectListModel, ProjectDetailModel, ProjectEntityMapper>(unitOfWorkFactory, modelMapper),
        IProjectFacade
{
    public override async Task<IEnumerable<ProjectListModel>> GetAsync()
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<ProjectEntity> entities = await uow
            .GetRepository<ProjectEntity, ProjectEntityMapper>()
            .Get()
            .OrderBy(p => p.Name)
            .ToListAsync()
            .ConfigureAwait(false);

        return ModelMapper.MapToListModel(entities);
    }
}
