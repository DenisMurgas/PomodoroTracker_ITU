// Autor: Tým / Spoločné

using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.DAL.Seeds;

public static class ProjectSeeds
{
    public static readonly ProjectEntity ClientWeb = new()
    {
        Id = Guid.Parse("287af1dc-0335-4cac-b3ff-99f9255f16cc"),
        Name = "Coding",
        Color = "#52622D"
    };

    public static readonly ProjectEntity SchoolItu = new()
    {
        Id = Guid.Parse("ae214c41-3e62-4cd3-ad2d-c3b8b52d3c03"),
        Name = "Škola",
        Color = "#7C3AED"
    };

    public static readonly ProjectEntity Guitar = new()
    {
        Id = Guid.Parse("2308561f-7431-421a-8490-1c510f4a72be"),
        Name = "Gitara",
        Color = "#15803D"
    };

    public static IReadOnlyList<ProjectEntity> All => [ClientWeb, SchoolItu, Guitar];
}
