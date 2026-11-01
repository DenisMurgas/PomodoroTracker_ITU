// Autor: xmurgad00

using CommunityToolkit.Mvvm.ComponentModel;

namespace PomodoroTracker.BL.Models;

public partial class TaskListModel : ModelBase
{
    [ObservableProperty]
    public required partial string Name { get; set; }

    [ObservableProperty]
    public partial bool IsCompleted { get; set; }

    [ObservableProperty]
    public required partial Guid ProjectId { get; set; }

    [ObservableProperty]
    public required partial string ProjectName { get; set; }

    [ObservableProperty]
    public required partial string ProjectColor { get; set; }

    [ObservableProperty]
    public partial TimeSpan TrackedTime { get; set; }

    [ObservableProperty]
    public partial int WorkBlockCount { get; set; }

    [ObservableProperty]
    public partial DateTime? LastWorkedAt { get; set; }

    public static TaskListModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        ProjectId = Guid.Empty,
        ProjectName = string.Empty,
        ProjectColor = string.Empty
    };
}
