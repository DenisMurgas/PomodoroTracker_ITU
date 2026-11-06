// Autor: xmurgad00

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PomodoroTracker.App.Messages;
using PomodoroTracker.App.Models;
using PomodoroTracker.App.Services;
using PomodoroTracker.BL.Calculations;
using PomodoroTracker.BL.Facades;
using PomodoroTracker.BL.Models;
using PomodoroTracker.BL.Timer;
using PomodoroTracker.Common.Enums;

namespace PomodoroTracker.App.ViewModels;

public partial class TaskListViewModel(
    ITaskFacade taskFacade,
    IProjectFacade projectFacade,
    IStatisticsFacade statisticsFacade,
    IPomodoroTimerService timerService,
    INavigationService navigationService,
    TimeProvider timeProvider,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<SessionFinishedMessage>
{
    private static readonly string[] ProjectColors =
    [
        "#2563EB", "#7C3AED", "#15803D", "#52622D", "#D97706", "#DB2777", "#0891B2", "#B45309"
    ];

    private List<TaskListModel> _allTasks = [];
    private Guid? _lastWorkedTaskId;

    [ObservableProperty]
    public partial IReadOnlyList<ProjectFilterModel> Filters { get; set; } = [];

    [ObservableProperty]
    public partial IReadOnlyList<ProjectListModel> Projects { get; set; } = [];

    [ObservableProperty]
    public partial IReadOnlyList<TaskRowModel> Tasks { get; set; } = [];

    [ObservableProperty]
    public partial TimeSpan TodayWorkTime { get; set; }

    [ObservableProperty]
    public partial bool HasCompletedTasks { get; set; }

    [ObservableProperty]
    public partial string CompletedCountText { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddTaskCommand))]
    public partial string NewTaskName { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddTaskCommand))]
    public partial ProjectListModel? NewTaskProject { get; set; }

    [ObservableProperty]
    public partial bool IsAddingProject { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddProjectCommand))]
    public partial string NewProjectName { get; set; } = string.Empty;

    private Guid? SelectedProjectId => Filters.FirstOrDefault(f => f.IsSelected)?.ProjectId;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        await ReloadProjectsAsync();
        await ReloadTasksAsync();
    }

    public async void Receive(SessionFinishedMessage message)
    {
        await ReloadTasksAsync();
    }

    [RelayCommand]
    private void SelectFilter(ProjectFilterModel filter)
    {
        foreach (ProjectFilterModel item in Filters)
        {
            item.IsSelected = item == filter;
        }

        ApplyFilter();
    }

    [RelayCommand(CanExecute = nameof(CanAddTask))]
    private async Task AddTaskAsync()
    {
        TaskDetailModel saved = await taskFacade.SaveAsync(new TaskDetailModel
        {
            Id = Guid.Empty,
            Name = NewTaskName.Trim(),
            ProjectId = NewTaskProject!.Id
        });

        NewTaskName = string.Empty;
        await ReloadTasksAsync();

        MessengerService.Send(new TaskChangedMessage { TaskId = saved.Id });
    }

    private bool CanAddTask()
        => !string.IsNullOrWhiteSpace(NewTaskName) && NewTaskProject is not null;

    [RelayCommand]
    private async Task CompleteTaskAsync(TaskRowModel row)
    {
        await taskFacade.SetCompletedAsync(row.Item.Id, true);
        await ReloadTasksAsync();

        MessengerService.Send(new TaskChangedMessage { TaskId = row.Item.Id });
    }

    [RelayCommand]
    private async Task DeleteTaskAsync(TaskRowModel row)
    {
        if (timerService.CurrentTaskId == row.Item.Id)
        {
            await timerService.SetTaskAsync(null);
        }

        await taskFacade.DeleteAsync(row.Item.Id);
        await ReloadTasksAsync();

        MessengerService.Send(new TaskChangedMessage { TaskId = row.Item.Id });
    }

    [RelayCommand]
    private async Task StartTaskAsync(TaskRowModel row)
    {
        await timerService.SetTaskAsync(row.Item.Id);

        await navigationService.GoToAsync(NavigationService.TimerRouteAbsolute);
    }

    [RelayCommand]
    private void StartAddingProject()
    {
        NewProjectName = string.Empty;
        IsAddingProject = true;
    }

    [RelayCommand]
    private void CancelAddingProject()
    {
        IsAddingProject = false;
        NewProjectName = string.Empty;
    }

    [RelayCommand(CanExecute = nameof(CanAddProject))]
    private async Task AddProjectAsync()
    {
        ProjectDetailModel saved = await projectFacade.SaveAsync(new ProjectDetailModel
        {
            Id = Guid.Empty,
            Name = NewProjectName.Trim(),
            Color = PickProjectColor()
        });

        IsAddingProject = false;
        NewProjectName = string.Empty;

        await ReloadProjectsAsync();
        NewTaskProject = Projects.FirstOrDefault(p => p.Id == saved.Id) ?? NewTaskProject;
    }

    private bool CanAddProject()
        => !string.IsNullOrWhiteSpace(NewProjectName);

    private async Task ReloadProjectsAsync()
    {
        Guid? selectedFilterId = SelectedProjectId;
        Guid? selectedNewTaskProjectId = NewTaskProject?.Id;

        Projects = (await projectFacade.GetAsync()).ToList();
        NewTaskProject = Projects.FirstOrDefault(p => p.Id == selectedNewTaskProjectId) ?? Projects.FirstOrDefault();

        ProjectFilterModel all = new(null, "Všetky", string.Empty);
        Filters = new[] { all }
            .Concat(Projects.Select(p => new ProjectFilterModel(p.Id, p.Name, p.Color)))
            .ToList();

        ProjectFilterModel selected = Filters.FirstOrDefault(f => f.ProjectId == selectedFilterId) ?? all;
        selected.IsSelected = true;
    }

    private async Task ReloadTasksAsync()
    {
        _allTasks = (await taskFacade.GetAsync()).ToList();
        _lastWorkedTaskId = (await taskFacade.GetLastWorkedAsync())?.Id;

        (DateTime from, DateTime to) = StatisticsCalculator.GetRange(StatisticsPeriod.Day, timeProvider.GetLocalNow().DateTime);
        TodayWorkTime = (await statisticsFacade.GetSummaryAsync(from, to)).WorkTime;

        ApplyFilter();
    }

    private void ApplyFilter()
    {
        Guid? projectId = SelectedProjectId;
        List<TaskListModel> visible = _allTasks
            .Where(t => projectId is null || t.ProjectId == projectId)
            .ToList();

        Tasks = visible
            .Where(t => !t.IsCompleted)
            .Select(t => new TaskRowModel(t, t.Id == _lastWorkedTaskId))
            .ToList();

        int completedCount = visible.Count(t => t.IsCompleted);
        HasCompletedTasks = completedCount > 0;
        CompletedCountText = FormatTaskCount(completedCount);
    }

    private string PickProjectColor()
    {
        HashSet<string> used = Projects.Select(p => p.Color).ToHashSet(StringComparer.OrdinalIgnoreCase);
        return ProjectColors.FirstOrDefault(c => !used.Contains(c))
               ?? ProjectColors[Projects.Count % ProjectColors.Length];
    }

    private static string FormatTaskCount(int count) => count switch
    {
        1 => "1 úloha",
        >= 2 and <= 4 => $"{count} úlohy",
        _ => $"{count} úloh"
    };
}
