// Autor: xpeterm01

using CommunityToolkit.Mvvm.ComponentModel;
using PomodoroTracker.Common.Enums;

namespace PomodoroTracker.BL.Models;

public partial class SessionListModel : ModelBase
{
    [ObservableProperty]
    public required partial SessionType Type { get; set; }

    [ObservableProperty]
    public required partial DateTime StartedAt { get; set; }

    [ObservableProperty]
    public partial DateTime? EndedAt { get; set; }

    [ObservableProperty]
    public required partial TimeSpan PlannedDuration { get; set; }

    [ObservableProperty]
    public partial TimeSpan ActualDuration { get; set; }

    [ObservableProperty]
    public partial TimeSpan Overrun { get; set; }

    [ObservableProperty]
    public partial Guid? TaskId { get; set; }

    [ObservableProperty]
    public partial string? TaskName { get; set; }

    public static SessionListModel Empty => new()
    {
        Id = Guid.Empty,
        Type = SessionType.Work,
        StartedAt = DateTime.MinValue,
        PlannedDuration = TimeSpan.Zero
    };
}
