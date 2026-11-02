// Autor: xpeterm01

using System.ComponentModel;
using PomodoroTracker.BL.Models;
using PomodoroTracker.Common.Enums;

namespace PomodoroTracker.BL.Timer;

public interface IPomodoroTimerService : INotifyPropertyChanged
{
    TimerState State { get; }
    SessionType CurrentPhase { get; }
    TimeSpan PlannedDuration { get; }
    TimeSpan Elapsed { get; }
    TimeSpan Remaining { get; }
    TimeSpan Overrun { get; }
    int CompletedWorkBlocks { get; }
    TimerProfileModel? Profile { get; }
    Guid? CurrentTaskId { get; }
    bool AutoStartNext { get; set; }

    event EventHandler<SessionType>? PlannedTimeElapsed;
    event EventHandler<SessionDetailModel>? SessionFinished;

    Task InitializeAsync();
    void SetProfile(TimerProfileModel profile);
    Task SetTaskAsync(Guid? taskId);
    Task StartTaskAsync(Guid taskId);
    Task StartAsync();
    void Pause();
    void Resume();
    void Extend(TimeSpan duration);
    Task SkipAsync();
    Task ReturnToWorkAsync();
    Task StopAsync();
    Task RefreshAsync();
}
