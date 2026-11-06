// Autor: Tým / Spoločné

using PomodoroTracker.App.ViewModels;

namespace PomodoroTracker.App.Views.Statistics;

public partial class StatisticsView : ContentPageBase
{
    public StatisticsView(StatisticsViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}
