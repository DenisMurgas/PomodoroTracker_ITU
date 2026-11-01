// Autor: xpeterm01

using CommunityToolkit.Mvvm.ComponentModel;

namespace PomodoroTracker.BL.Models;

public partial class TimerProfileModel : ModelBase
{
    [ObservableProperty]
    public required partial string Name { get; set; }

    [ObservableProperty]
    public required partial TimeSpan WorkDuration { get; set; }

    [ObservableProperty]
    public required partial TimeSpan ShortBreakDuration { get; set; }

    [ObservableProperty]
    public required partial TimeSpan LongBreakDuration { get; set; }

    [ObservableProperty]
    public required partial int LongBreakInterval { get; set; }

    public static TimerProfileModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        WorkDuration = TimeSpan.FromMinutes(25),
        ShortBreakDuration = TimeSpan.FromMinutes(5),
        LongBreakDuration = TimeSpan.FromMinutes(15),
        LongBreakInterval = 4
    };
}
