// Autor: Tým / Spoločné

using CommunityToolkit.Mvvm.Messaging;
using PomodoroTracker.App.Extensions;
using PomodoroTracker.App.Services;
using PomodoroTracker.App.Shells;

namespace PomodoroTracker.App;

public static class AppInstaller
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<AppShell>();

        services.AddSingleton<IMessenger>(_ => WeakReferenceMessenger.Default);

        services.AddSingleton<IMessengerService, MessengerService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IAlertService, AlertService>();
        services.AddSingleton<ITimerMessageBridge, TimerMessageBridge>();

        services.AddViews();
        services.AddViewModels();

        return services;
    }
}
