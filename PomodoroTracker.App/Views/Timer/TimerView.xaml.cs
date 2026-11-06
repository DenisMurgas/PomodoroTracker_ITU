// Autor: Tým / Spoločné

using PomodoroTracker.App.ViewModels;

namespace PomodoroTracker.App.Views.Timer;

public partial class TimerView : ContentPageBase
{
    public TimerView(TimerViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}
