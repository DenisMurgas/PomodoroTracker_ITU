// Autor: Tým / Spoločné

using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.DAL.Seeds;

public static class TaskSeeds
{
    private static readonly DateTime Today = DateTime.Today;

    public static readonly TaskEntity CodeReview = new()
    {
        Id = Guid.Parse("9831bc2f-a5d1-43e5-a627-6fb3f847a0e8"),
        Name = "Code review #42",
        CreatedAt = Today.AddDays(-1),
        ProjectId = ProjectSeeds.ClientWeb.Id
    };

    public static readonly TaskEntity ApiTests = new()
    {
        Id = Guid.Parse("26736e19-9767-43ac-ba67-09b91b1c1b1b"),
        Name = "Napísať testy API",
        CreatedAt = Today.AddDays(-1),
        ProjectId = ProjectSeeds.ClientWeb.Id
    };

    public static readonly TaskEntity LoginRefactor = new()
    {
        Id = Guid.Parse("511d8473-b45a-4eba-a499-66dd65a71aae"),
        Name = "Refaktor prihlasovania",
        CreatedAt = Today.AddDays(-3),
        IsCompleted = true,
        CompletedAt = Today.AddHours(10).AddMinutes(30),
        ProjectId = ProjectSeeds.ClientWeb.Id
    };

    public static readonly TaskEntity Wireframes = new()
    {
        Id = Guid.Parse("0ec69b71-2bf0-4e10-82e5-de4dafc36f27"),
        Name = "Wireframy obrazoviek",
        CreatedAt = Today.AddDays(-2),
        ProjectId = ProjectSeeds.SchoolItu.Id
    };

    public static readonly TaskEntity EntityDiagram = new()
    {
        Id = Guid.Parse("8b70b437-b869-4246-8221-67a4f872cc77"),
        Name = "Diagram entít",
        CreatedAt = Today.AddDays(-2),
        IsCompleted = true,
        CompletedAt = Today.AddHours(13).AddMinutes(20),
        ProjectId = ProjectSeeds.SchoolItu.Id
    };

    public static readonly TaskEntity GuitarScales = new()
    {
        Id = Guid.Parse("9aba2030-dcb3-42e4-8d84-5d4af222d58d"),
        Name = "Stupnice a akordy",
        CreatedAt = Today.AddDays(-5),
        ProjectId = ProjectSeeds.Guitar.Id
    };

    public static IReadOnlyList<TaskEntity> All =>
        [CodeReview, ApiTests, LoginRefactor, Wireframes, EntityDiagram, GuitarScales];
}
