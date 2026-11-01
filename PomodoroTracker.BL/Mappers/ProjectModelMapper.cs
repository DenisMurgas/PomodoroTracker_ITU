// Autor: xmurgad00

using PomodoroTracker.BL.Models;
using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.BL.Mappers;

public class ProjectModelMapper : ModelMapperBase<ProjectEntity, ProjectListModel, ProjectDetailModel>
{
    public override ProjectListModel MapToListModel(ProjectEntity? entity)
        => entity is null
            ? ProjectListModel.Empty
            : new ProjectListModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Color = entity.Color
            };

    public override ProjectDetailModel MapToDetailModel(ProjectEntity? entity)
        => entity is null
            ? ProjectDetailModel.Empty
            : new ProjectDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Color = entity.Color
            };

    public override ProjectEntity MapToEntity(ProjectDetailModel model)
        => new() { Id = model.Id, Name = model.Name, Color = model.Color };
}
