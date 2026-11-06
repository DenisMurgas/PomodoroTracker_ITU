// Autor: Tým / Spoločné

using PomodoroTracker.App.ViewModels;

namespace PomodoroTracker.App.Views;

public abstract partial class ContentPageBase
{
    protected ViewModelBase ViewModel { get; }

    protected ContentPageBase(ViewModelBase viewModel)
    {
        InitializeComponent();

        BindingContext = ViewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await ViewModel.OnAppearingAsync();
    }
}
