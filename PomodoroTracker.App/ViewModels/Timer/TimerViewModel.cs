// Autor: xpeterm01

using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PomodoroTracker.App.Messages;
using PomodoroTracker.App.Models;
using PomodoroTracker.App.Services;
using PomodoroTracker.BL.Calculations;
using PomodoroTracker.BL.Facades;
using PomodoroTracker.BL.Models;
using PomodoroTracker.BL.Models.Statistics;
using PomodoroTracker.BL.Timer;
using PomodoroTracker.Common.Enums;

namespace PomodoroTracker.App.ViewModels;

public partial class TimerViewModel : ViewModelBase, IRecipient<TaskChangedMessage>, IRecipient<SessionFinishedMessage>
{
    private const int MinLongBreakInterval = 1;
    private const int MaxLongBreakInterval = 6;

    private static readonly TimeSpan ExtendStep = TimeSpan.FromMinutes(5);

    private static readonly string[] TimerDependentProperties =
    [
        nameof(ClockText), nameof(PhaseText), nameof(BlockText), nameof(Progress),
        nameof(IsBreakPhase), nameof(IsOverrun), nameof(IsIdle), nameof(IsRunning), nameof(IsPaused),
        nameof(IsWorkActive), nameof(IsBreakActive), nameof(StartText),
        nameof(TodayWorkTime), nameof(TodayBreakTime), nameof(TodayOverrunTime)
    ];

    private readonly IPomodoroTimerService _timerService;
    private readonly ITaskFacade _taskFacade;
    private readonly ITimerProfileFacade _timerProfileFacade;
    private readonly ISettingsFacade _settingsFacade;
    private readonly IStatisticsFacade _statisticsFacade;
    private readonly INavigationService _navigationService;
    private readonly TimeProvider _timeProvider;

    private SettingsModel? _settings;
    private bool _isSyncing;

    public TimerViewModel(
        IPomodoroTimerService timerService,
        ITaskFacade taskFacade,
        ITimerProfileFacade timerProfileFacade,
        ISettingsFacade settingsFacade,
        IStatisticsFacade statisticsFacade,
        INavigationService navigationService,
        TimeProvider timeProvider,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _timerService = timerService;
        _taskFacade = taskFacade;
        _timerProfileFacade = timerProfileFacade;
        _settingsFacade = settingsFacade;
        _statisticsFacade = statisticsFacade;
        _navigationService = navigationService;
        _timeProvider = timeProvider;

        _timerService.PropertyChanged += OnTimerPropertyChanged;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasTask))]
    public partial TaskListModel? CurrentTask { get; set; }

    [ObservableProperty]
    public partial bool IsPanelOpen { get; set; } = true;

    [ObservableProperty]
    public partial IReadOnlyList<TimerProfileOptionModel> Profiles { get; set; } = [];

    [ObservableProperty]
    public partial int LongBreakIndex { get; set; } = -1;

    [ObservableProperty]
    public partial bool AutoStartNext { get; set; }

    [ObservableProperty]
    public partial bool NotificationsEnabled { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TodayWorkTime), nameof(TodayBreakTime), nameof(TodayOverrunTime))]
    public partial StatisticsSummaryModel Today { get; set; } = new(TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, 0, 0);

    public IReadOnlyList<string> LongBreakOptions { get; } = Enumerable
        .Range(MinLongBreakInterval, MaxLongBreakInterval - MinLongBreakInterval + 1)
        .Select(n => n == 1 ? "1 bloku" : $"{n} blokoch")
        .ToList();

    public bool HasTask => CurrentTask is not null;

    public TimeSpan TodayWorkTime => Today.WorkTime + (IsWorkActive ? _timerService.Elapsed : TimeSpan.Zero);

    public TimeSpan TodayBreakTime => Today.BreakTime + (IsBreakPhase && !IsIdle ? _timerService.Elapsed - _timerService.Overrun : TimeSpan.Zero);

    public TimeSpan TodayOverrunTime => Today.OverrunTime + _timerService.Overrun;

    public string ClockText => IsOverrun
        ? "+" + FormatClock(_timerService.Overrun)
        : FormatClock(_timerService.Remaining);

    public string PhaseText => _timerService.State switch
    {
        TimerState.BreakOverrun => "Prestávka skončila",
        TimerState.Paused => "Pozastavené",
        TimerState.Idle when !IsBreakPhase => "Pripravené",
        _ when IsBreakPhase => $"Prestávka {(int)_timerService.PlannedDuration.TotalMinutes} min",
        _ => "Práca"
    };

    public string BlockText
    {
        get
        {
            int interval = _timerService.Profile?.LongBreakInterval ?? 4;
            int block = IsBreakPhase ? _timerService.CompletedWorkBlocks : _timerService.CompletedWorkBlocks + 1;
            return $"blok {Math.Clamp(block, 1, interval)} zo {interval}";
        }
    }

    public double Progress
    {
        get
        {
            if (IsOverrun)
            {
                return 1;
            }

            TimeSpan planned = _timerService.PlannedDuration;
            return planned <= TimeSpan.Zero ? 0 : Math.Clamp(_timerService.Elapsed / planned, 0, 1);
        }
    }

    public bool IsBreakPhase => _timerService.CurrentPhase != SessionType.Work;

    public bool IsOverrun => _timerService.State == TimerState.BreakOverrun;

    public bool IsIdle => _timerService.State == TimerState.Idle;

    public bool IsRunning => _timerService.State == TimerState.Running;

    public bool IsPaused => _timerService.State == TimerState.Paused;

    public bool IsWorkActive => !IsIdle && !IsBreakPhase;

    public bool IsBreakActive => (IsRunning || IsPaused) && IsBreakPhase;

    public string StartText => IsBreakPhase ? "Spustiť prestávku" : "Spustiť";

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        _settings = await _settingsFacade.GetCurrentAsync();
        Sync(() =>
        {
            AutoStartNext = _timerService.AutoStartNext;
            NotificationsEnabled = _settings.NotificationsEnabled;
        });

        await ReloadProfilesAsync();
        await LoadCurrentTaskAsync();
        await LoadTodayAsync();
    }

    public async void Receive(TaskChangedMessage message)
    {
        if (message.TaskId == _timerService.CurrentTaskId || message.TaskId == CurrentTask?.Id)
        {
            await LoadCurrentTaskAsync();
        }
    }

    public async void Receive(SessionFinishedMessage message)
    {
        await LoadCurrentTaskAsync();
        await LoadTodayAsync();
    }

    [RelayCommand]
    private Task StartAsync() => _timerService.StartAsync();

    [RelayCommand]
    private void Pause() => _timerService.Pause();

    [RelayCommand]
    private void Resume() => _timerService.Resume();

    [RelayCommand]
    private void Extend() => _timerService.Extend(ExtendStep);

    [RelayCommand]
    private Task SkipAsync() => _timerService.SkipAsync();

    [RelayCommand]
    private Task ReturnToWorkAsync() => _timerService.ReturnToWorkAsync();

    [RelayCommand]
    private void TogglePanel() => IsPanelOpen = !IsPanelOpen;

    [RelayCommand]
    private void OpenPanel() => IsPanelOpen = true;

    [RelayCommand]
    private void ToggleAutoStart() => AutoStartNext = !AutoStartNext;

    [RelayCommand]
    private void ToggleNotifications() => NotificationsEnabled = !NotificationsEnabled;

    [RelayCommand]
    private Task ChangeTaskAsync() => _navigationService.GoToAsync(NavigationService.TasksRouteAbsolute);

    [RelayCommand]
    private async Task SelectProfileAsync(TimerProfileOptionModel option)
    {
        _timerService.SetProfile(option.Profile);

        if (_settings is not null)
        {
            _settings.ActiveTimerProfileId = option.Profile.Id;
            _settings = await _settingsFacade.SaveAsync(_settings);
        }
    }

    partial void OnLongBreakIndexChanged(int value)
    {
        if (!_isSyncing && value >= 0)
        {
            SaveLongBreakInterval(MinLongBreakInterval + value);
        }
    }

    partial void OnAutoStartNextChanged(bool value)
    {
        if (!_isSyncing)
        {
            _timerService.AutoStartNext = value;
            SaveSettings();
        }
    }

    partial void OnNotificationsEnabledChanged(bool value)
    {
        if (!_isSyncing)
        {
            SaveSettings();
        }
    }

    private async void SaveLongBreakInterval(int interval)
    {
        TimerProfileModel? profile = _timerService.Profile;
        if (profile is null || profile.LongBreakInterval == interval)
        {
            return;
        }

        profile.LongBreakInterval = interval;
        await _timerProfileFacade.SaveAsync(profile);
        await ReloadProfilesAsync();
    }

    private async void SaveSettings()
    {
        if (_settings is null)
        {
            return;
        }

        _settings.AutoStartNext = AutoStartNext;
        _settings.NotificationsEnabled = NotificationsEnabled;
        _settings = await _settingsFacade.SaveAsync(_settings);
    }

    private async Task ReloadProfilesAsync()
    {
        Profiles = (await _timerProfileFacade.GetAsync())
            .OrderBy(p => p.WorkDuration)
            .Select(p => new TimerProfileOptionModel(p))
            .ToList();

        Guid? activeId = _timerService.Profile?.Id ?? _settings?.ActiveTimerProfileId;
        TimerProfileOptionModel? active = Profiles.FirstOrDefault(p => p.Profile.Id == activeId);
        if (active is not null)
        {
            _timerService.SetProfile(active.Profile);
        }

        UpdateProfileSelection();
    }

    private void UpdateProfileSelection()
    {
        Guid? activeId = _timerService.Profile?.Id;
        foreach (TimerProfileOptionModel option in Profiles)
        {
            option.IsSelected = option.Profile.Id == activeId;
        }

        int interval = _timerService.Profile?.LongBreakInterval ?? -1;
        Sync(() => LongBreakIndex = interval < MinLongBreakInterval ? -1 : Math.Min(interval, MaxLongBreakInterval) - MinLongBreakInterval);
    }

    private async void OnTimerPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        foreach (string property in TimerDependentProperties)
        {
            OnPropertyChanged(property);
        }

        switch (e.PropertyName)
        {
            case nameof(IPomodoroTimerService.CurrentTaskId):
                await LoadCurrentTaskAsync();
                break;
            case nameof(IPomodoroTimerService.Profile):
                UpdateProfileSelection();
                break;
            case nameof(IPomodoroTimerService.AutoStartNext):
                Sync(() => AutoStartNext = _timerService.AutoStartNext);
                break;
        }
    }

    private async Task LoadCurrentTaskAsync()
    {
        Guid? taskId = _timerService.CurrentTaskId;
        CurrentTask = taskId is null
            ? null
            : (await _taskFacade.GetAsync()).FirstOrDefault(t => t.Id == taskId);
    }

    private async Task LoadTodayAsync()
    {
        (DateTime from, DateTime to) = StatisticsCalculator.GetRange(StatisticsPeriod.Day, _timeProvider.GetLocalNow().DateTime);
        Today = await _statisticsFacade.GetSummaryAsync(from, to);
    }

    private void Sync(Action action)
    {
        _isSyncing = true;
        try
        {
            action();
        }
        finally
        {
            _isSyncing = false;
        }
    }

    private static string FormatClock(TimeSpan time)
        => $"{(int)time.TotalMinutes:00}:{time.Seconds:00}";
}
