// Autor: xpeterm01

using CommunityToolkit.Mvvm.ComponentModel;
using PomodoroTracker.BL.Models;

namespace PomodoroTracker.App.Models;

public partial class TimerProfileOptionModel(TimerProfileModel profile) : ObservableObject
{
    public TimerProfileModel Profile { get; } = profile;

    [ObservableProperty]
    public partial bool IsSelected { get; set; }
}
