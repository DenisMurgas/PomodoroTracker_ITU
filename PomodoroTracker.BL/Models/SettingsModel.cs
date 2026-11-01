// Autor: xpeterm01

using CommunityToolkit.Mvvm.ComponentModel;

namespace PomodoroTracker.BL.Models;

public partial class SettingsModel : ModelBase
{
    [ObservableProperty]
    public partial bool AutoStartNext { get; set; }

    [ObservableProperty]
    public partial bool NotificationsEnabled { get; set; }

    [ObservableProperty]
    public partial Guid? ActiveTimerProfileId { get; set; }

    public static SettingsModel Empty => new()
    {
        Id = Guid.Empty,
        AutoStartNext = true,
        NotificationsEnabled = true
    };
}
