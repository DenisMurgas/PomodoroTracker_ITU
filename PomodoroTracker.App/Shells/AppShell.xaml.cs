// Autor: Tým / Spoločné

using CommunityToolkit.Mvvm.Input;
using PomodoroTracker.App.Services;

namespace PomodoroTracker.App.Shells;

public partial class AppShell
{
    private readonly INavigationService _navigationService;
    private string _currentRoute = NavigationService.TimerRoute;

    public AppShell(INavigationService navigationService)
    {
        _navigationService = navigationService;

        InitializeComponent();
    }

    public string CurrentRoute
    {
        get => _currentRoute;
        private set
        {
            if (_currentRoute == value)
            {
                return;
            }

            _currentRoute = value;
            OnPropertyChanged();
        }
    }

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);

        CurrentRoute = CurrentItem?.CurrentItem?.CurrentItem?.Route ?? string.Empty;
    }

    [RelayCommand]
    private async Task NavigateAsync(string route)
        => await _navigationService.GoToAsync($"//{route}");
}
