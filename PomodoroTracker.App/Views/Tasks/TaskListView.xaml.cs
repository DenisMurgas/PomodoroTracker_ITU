// Autor: xmurgad00

using PomodoroTracker.App.ViewModels;

namespace PomodoroTracker.App.Views.Tasks;

public partial class TaskListView : ContentPageBase
{
    public TaskListView(TaskListViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}
