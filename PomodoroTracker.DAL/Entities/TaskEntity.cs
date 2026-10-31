// Autor: Tým / Spoločné

namespace PomodoroTracker.DAL.Entities;

public record TaskEntity : IEntity
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    public bool IsCompleted { get; set; }

    public required DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public required Guid ProjectId { get; set; }

    public ProjectEntity? Project { get; init; }

    public ICollection<SessionEntity> Sessions { get; init; } = new List<SessionEntity>();
}
