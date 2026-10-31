// Autor: Tým / Spoločné

namespace PomodoroTracker.DAL.Entities;

public record ProjectEntity : IEntity
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Color { get; set; }

    public ICollection<TaskEntity> Tasks { get; init; } = new List<TaskEntity>();
}
