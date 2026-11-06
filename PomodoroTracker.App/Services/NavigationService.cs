// Autor: xmurgad00

using PomodoroTracker.App.Models;

namespace PomodoroTracker.App.Services;

public class NavigationService : INavigationService
{
    public const string TimerRoute = "timer";
    public const string TasksRoute = "tasks";
    public const string StatisticsRoute = "statistics";

    public const string TimerRouteAbsolute = "//" + TimerRoute;
    public const string TasksRouteAbsolute = "//" + TasksRoute;
    public const string StatisticsRouteAbsolute = "//" + StatisticsRoute;

    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>();

    public async Task GoToAsync(string route)
        => await Shell.Current.GoToAsync(route);

    public async Task GoToAsync(string route, IDictionary<string, object?> parameters)
        => await Shell.Current.GoToAsync(route, parameters);

    public bool SendBackButtonPressed()
        => Shell.Current.SendBackButtonPressed();
}
