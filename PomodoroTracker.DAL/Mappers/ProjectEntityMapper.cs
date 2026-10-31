// Autor: xmurgad00

using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.DAL.Mappers;

public class ProjectEntityMapper : IEntityMapper<ProjectEntity>
{
    public void MapToExistingEntity(ProjectEntity existingEntity, ProjectEntity newEntity)
    {
        existingEntity.Name = newEntity.Name;
        existingEntity.Color = newEntity.Color;
    }
}
