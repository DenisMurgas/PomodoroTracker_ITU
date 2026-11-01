// Autor: xmurgad00

using CommunityToolkit.Mvvm.ComponentModel;

namespace PomodoroTracker.BL.Models;

public partial class ProjectDetailModel : ModelBase
{
    [ObservableProperty]
    public required partial string Name { get; set; }

    [ObservableProperty]
    public required partial string Color { get; set; }

    public static ProjectDetailModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        Color = "#2563EB"
    };
}
