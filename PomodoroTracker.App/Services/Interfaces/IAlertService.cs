// Autor: xmurgad00

namespace PomodoroTracker.App.Services;

public interface IAlertService
{
    Task DisplayAsync(string title, string message);
}
