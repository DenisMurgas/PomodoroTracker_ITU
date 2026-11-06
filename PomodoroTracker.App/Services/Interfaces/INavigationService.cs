// Autor: xmurgad00

using PomodoroTracker.App.Models;

namespace PomodoroTracker.App.Services;

public interface INavigationService
{
    IEnumerable<RouteModel> Routes { get; }

    Task GoToAsync(string route);
    Task GoToAsync(string route, IDictionary<string, object?> parameters);
    bool SendBackButtonPressed();
}
