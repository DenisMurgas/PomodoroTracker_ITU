// Autor: xmurgad00

namespace PomodoroTracker.App.Services;

public class AlertService : IAlertService
{
    public async Task DisplayAsync(string title, string message)
    {
        Page? page = Application.Current?.Windows.FirstOrDefault()?.Page;
        if (page is null)
        {
            return;
        }

        await page.DisplayAlertAsync(title, message, "OK");
    }
}
