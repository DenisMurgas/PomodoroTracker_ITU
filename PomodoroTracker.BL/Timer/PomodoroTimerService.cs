// Autor: xpeterm01

using CommunityToolkit.Mvvm.ComponentModel;
using PomodoroTracker.BL.Facades;
using PomodoroTracker.BL.Models;
using PomodoroTracker.Common.Enums;

namespace PomodoroTracker.BL.Timer;

public partial class PomodoroTimerService(
    ISessionFacade sessionFacade,
    ISettingsFacade settingsFacade,
    ITimerProfileFacade timerProfileFacade,
    TimeProvider timeProvider)
    : ObservableObject, IPomodoroTimerService, IDisposable
{
    private static readonly TimeSpan TickInterval = TimeSpan.FromSeconds(1);

    private ITimer? _timer;
    private SynchronizationContext? _synchronizationContext;
    private SessionDetailModel? _currentSession;
    private DateTime _phaseStartedAt;
    private TimeSpan _pausedTotal;
    private DateTime? _pausedAt;
    private bool _isTransitioning;

    [ObservableProperty]
    public partial TimerState State { get; private set; } = TimerState.Idle;

    [ObservableProperty]
    public partial SessionType CurrentPhase { get; private set; } = SessionType.Work;

    [ObservableProperty]
    public partial TimeSpan PlannedDuration { get; private set; }

    [ObservableProperty]
    public partial TimeSpan Elapsed { get; private set; }

    [ObservableProperty]
    public partial TimeSpan Remaining { get; private set; }

    [ObservableProperty]
    public partial TimeSpan Overrun { get; private set; }

    [ObservableProperty]
    public partial int CompletedWorkBlocks { get; private set; }

    [ObservableProperty]
    public partial TimerProfileModel? Profile { get; private set; }

    [ObservableProperty]
    public partial Guid? CurrentTaskId { get; private set; }

    [ObservableProperty]
    public partial bool AutoStartNext { get; set; } = true;

    public event EventHandler<SessionType>? PlannedTimeElapsed;
    public event EventHandler<SessionDetailModel>? SessionFinished;

    private bool IsActive => State is TimerState.Running or TimerState.Paused or TimerState.BreakOverrun;

    public async Task InitializeAsync()
    {
        await CloseUnfinishedSessionsAsync();

        SettingsModel settings = await settingsFacade.GetCurrentAsync();
        AutoStartNext = settings.AutoStartNext;

        TimerProfileModel? profile = settings.ActiveTimerProfileId is { } profileId
            ? await timerProfileFacade.GetAsync(profileId)
            : null;

        profile ??= (await timerProfileFacade.GetAsync()).FirstOrDefault();

        if (profile is not null)
        {
            SetProfile(profile);
        }
    }

    public void SetProfile(TimerProfileModel profile)
    {
        Profile = profile;

        if (State == TimerState.Idle)
        {
            ResetToIdle(CurrentPhase);
        }
    }

    public async Task SetTaskAsync(Guid? taskId)
    {
        if (CurrentTaskId == taskId)
        {
            return;
        }

        bool isWorking = State is (TimerState.Running or TimerState.Paused) && CurrentPhase == SessionType.Work;
        if (!isWorking || _currentSession is null)
        {
            CurrentTaskId = taskId;
            return;
        }

        bool wasPaused = State == TimerState.Paused;
        TimeSpan remaining = Remaining;

        await FinishCurrentSessionAsync(Now());
        CurrentTaskId = taskId;
        await StartPhaseAsync(SessionType.Work, remaining);

        if (wasPaused)
        {
            Pause();
        }
    }

    public async Task StartTaskAsync(Guid taskId)
    {
        await SetTaskAsync(taskId);

        if (State == TimerState.Idle)
        {
            if (CurrentPhase != SessionType.Work)
            {
                CompleteBreakCycle();
                ResetToIdle(SessionType.Work);
            }

            await StartAsync();
        }
        else if (CurrentPhase != SessionType.Work)
        {
            await ReturnToWorkAsync();
        }
        else if (State == TimerState.Paused)
        {
            Resume();
        }
    }

    public async Task StartAsync()
    {
        if (State == TimerState.Paused)
        {
            Resume();
            return;
        }

        if (State != TimerState.Idle)
        {
            return;
        }

        await StartPhaseAsync(CurrentPhase);
    }

    public void Pause()
    {
        if (State != TimerState.Running)
        {
            return;
        }

        _pausedAt = Now();
        State = TimerState.Paused;
    }

    public void Resume()
    {
        if (State != TimerState.Paused || _pausedAt is null)
        {
            return;
        }

        _pausedTotal += Now() - _pausedAt.Value;
        _pausedAt = null;
        State = TimerState.Running;
        UpdateTimes(Now());
    }

    public void Extend(TimeSpan duration)
    {
        if (State is not (TimerState.Running or TimerState.Paused) || CurrentPhase != SessionType.Work || duration <= TimeSpan.Zero)
        {
            return;
        }

        PlannedDuration += duration;
        UpdateTimes(Now());
    }

    public async Task SkipAsync()
    {
        if (State == TimerState.Idle)
        {
            if (CurrentPhase != SessionType.Work)
            {
                CompleteBreakCycle();
                ResetToIdle(SessionType.Work);
            }

            return;
        }

        if (CurrentPhase == SessionType.Work)
        {
            await FinishCurrentSessionAsync(Now());
            await ContinueAfterWorkAsync();
        }
        else
        {
            await ReturnToWorkAsync();
        }
    }

    public async Task ReturnToWorkAsync()
    {
        if (!IsActive || CurrentPhase == SessionType.Work)
        {
            return;
        }

        await FinishCurrentSessionAsync(Now());
        CompleteBreakCycle();
        await StartPhaseAsync(SessionType.Work);
    }

    public async Task StopAsync()
    {
        if (IsActive)
        {
            await FinishCurrentSessionAsync(Now());

            if (CurrentPhase != SessionType.Work)
            {
                CompleteBreakCycle();
            }
        }

        StopTimer();
        ResetToIdle(SessionType.Work);
    }

    public async Task RefreshAsync()
    {
        if (_isTransitioning || !IsActive)
        {
            return;
        }

        DateTime now = Now();
        UpdateTimes(now);

        if (State == TimerState.Running && Remaining <= TimeSpan.Zero)
        {
            _isTransitioning = true;
            try
            {
                await OnPlannedTimeElapsedAsync(now);
            }
            finally
            {
                _isTransitioning = false;
            }
        }
    }

    public void Dispose()
    {
        StopTimer();
        GC.SuppressFinalize(this);
    }

    private async Task OnPlannedTimeElapsedAsync(DateTime now)
    {
        SessionType phase = CurrentPhase;
        PlannedTimeElapsed?.Invoke(this, phase);

        if (phase == SessionType.Work)
        {
            await FinishCurrentSessionAsync(now);
            await ContinueAfterWorkAsync();
        }
        else
        {
            State = TimerState.BreakOverrun;
            UpdateTimes(now);
        }
    }

    private async Task ContinueAfterWorkAsync()
    {
        CompletedWorkBlocks++;

        SessionType nextBreak = Profile is not null && CompletedWorkBlocks >= Profile.LongBreakInterval
            ? SessionType.LongBreak
            : SessionType.ShortBreak;

        if (AutoStartNext)
        {
            await StartPhaseAsync(nextBreak);
        }
        else
        {
            ResetToIdle(nextBreak);
        }
    }

    private void CompleteBreakCycle()
    {
        if (CurrentPhase == SessionType.LongBreak)
        {
            CompletedWorkBlocks = 0;
        }
    }

    private async Task StartPhaseAsync(SessionType phase, TimeSpan? plannedOverride = null)
    {
        TimerProfileModel profile = Profile
                                    ?? throw new InvalidOperationException("Timer profile is not set.");

        TimeSpan planned = plannedOverride ?? DurationOf(profile, phase);
        DateTime now = Now();

        _currentSession = await sessionFacade.SaveAsync(new SessionDetailModel
        {
            Id = Guid.Empty,
            Type = phase,
            StartedAt = now,
            EndedAt = null,
            PlannedDuration = planned,
            PausedDuration = TimeSpan.Zero,
            TaskId = phase == SessionType.Work ? CurrentTaskId : null
        });

        _phaseStartedAt = now;
        _pausedTotal = TimeSpan.Zero;
        _pausedAt = null;

        CurrentPhase = phase;
        PlannedDuration = planned;
        State = TimerState.Running;
        UpdateTimes(now);
        EnsureTimer();
    }

    private async Task FinishCurrentSessionAsync(DateTime endedAt)
    {
        if (_currentSession is null)
        {
            return;
        }

        _currentSession.EndedAt = endedAt;
        _currentSession.PausedDuration = CurrentPausedDuration(endedAt);
        _currentSession.PlannedDuration = PlannedDuration;
        _currentSession.TaskId = CurrentPhase == SessionType.Work ? CurrentTaskId : null;

        SessionDetailModel finished = await sessionFacade.SaveAsync(_currentSession);
        _currentSession = null;

        SessionFinished?.Invoke(this, finished);
    }

    private async Task CloseUnfinishedSessionsAsync()
    {
        DateTime now = Now();

        foreach (SessionDetailModel session in await sessionFacade.GetUnfinishedAsync())
        {
            DateTime plannedEnd = session.StartedAt + session.PlannedDuration + session.PausedDuration;
            session.EndedAt = plannedEnd < now ? plannedEnd : now;
            await sessionFacade.SaveAsync(session);
        }
    }

    private void ResetToIdle(SessionType nextPhase)
    {
        _currentSession = null;
        _pausedAt = null;
        _pausedTotal = TimeSpan.Zero;

        CurrentPhase = nextPhase;
        State = TimerState.Idle;
        PlannedDuration = Profile is null ? TimeSpan.Zero : DurationOf(Profile, nextPhase);
        Elapsed = TimeSpan.Zero;
        Remaining = PlannedDuration;
        Overrun = TimeSpan.Zero;
    }

    private void UpdateTimes(DateTime now)
    {
        TimeSpan elapsed = now - _phaseStartedAt - CurrentPausedDuration(now);
        Elapsed = elapsed < TimeSpan.Zero ? TimeSpan.Zero : elapsed;

        TimeSpan remaining = PlannedDuration - Elapsed;
        Remaining = remaining < TimeSpan.Zero ? TimeSpan.Zero : remaining;

        Overrun = State == TimerState.BreakOverrun && Elapsed > PlannedDuration
            ? Elapsed - PlannedDuration
            : TimeSpan.Zero;
    }

    private TimeSpan CurrentPausedDuration(DateTime now)
        => _pausedAt is null ? _pausedTotal : _pausedTotal + (now - _pausedAt.Value);

    private static TimeSpan DurationOf(TimerProfileModel profile, SessionType phase)
        => phase switch
        {
            SessionType.Work => profile.WorkDuration,
            SessionType.ShortBreak => profile.ShortBreakDuration,
            SessionType.LongBreak => profile.LongBreakDuration,
            _ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null)
        };

    private DateTime Now() => timeProvider.GetLocalNow().DateTime;

    private void EnsureTimer()
    {
        _synchronizationContext ??= SynchronizationContext.Current;
        _timer ??= timeProvider.CreateTimer(_ => OnTimerTick(), null, TickInterval, TickInterval);
    }

    private void StopTimer()
    {
        _timer?.Dispose();
        _timer = null;
    }

    private void OnTimerTick()
    {
        if (_synchronizationContext is null)
        {
            _ = RefreshAsync();
        }
        else
        {
            _synchronizationContext.Post(_ => _ = RefreshAsync(), null);
        }
    }
}
