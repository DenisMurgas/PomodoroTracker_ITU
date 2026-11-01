// Autor: xmurgad00

using CommunityToolkit.Mvvm.ComponentModel;

namespace PomodoroTracker.BL.Models;

public partial class TaskDetailModel : ModelBase
{
    [ObservableProperty]
    public required partial string Name { get; set; }

    [ObservableProperty]
    public partial bool IsCompleted { get; set; }

    [ObservableProperty]
    public partial DateTime CreatedAt { get; set; }

    [ObservableProperty]
    public partial DateTime? CompletedAt { get; set; }

    [ObservableProperty]
    public required partial Guid ProjectId { get; set; }

    public static TaskDetailModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        ProjectId = Guid.Empty
    };
}
