// Autor: Tým / Spoločné

using System.Reflection;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Maui.LifecycleEvents;
using PomodoroTracker.App.Models;
using PomodoroTracker.App.Services;
using PomodoroTracker.BL;
using PomodoroTracker.DAL;
using PomodoroTracker.DAL.Migrator;
using PomodoroTracker.DAL.Options;
using PomodoroTracker.DAL.Seeds;

namespace PomodoroTracker.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Inter-Regular.otf", "InterRegular");
                fonts.AddFont("Inter-Medium.otf", "InterMedium");
                fonts.AddFont("Inter-SemiBold.otf", "InterSemiBold");
                fonts.AddFont("Inter-Bold.otf", "InterBold");
            });

        ConfigureWindow(builder);
        ConfigureAppSettings(builder);

        builder.Services
            .AddDALServices()
            .AddBLServices()
            .AddAppServices();

        MauiApp app = builder.Build();

        AssertDALOptionsConfiguration(app);
        app.Services.GetRequiredService<IDbMigrator>().Migrate();
        app.Services.GetRequiredService<IDbSeeder>().Seed();
        RegisterRouting(app.Services.GetRequiredService<INavigationService>());

        return app;
    }

    private static void ConfigureWindow(MauiAppBuilder builder)
    {
#if WINDOWS
        builder.ConfigureLifecycleEvents(events => events.AddWindows(windows => windows.OnWindowCreated(window =>
        {
            if (window.AppWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter presenter)
            {
                presenter.IsMaximizable = false;
            }
        })));
#endif
    }

    private static void ConfigureAppSettings(MauiAppBuilder builder)
    {
        ConfigurationBuilder configurationBuilder = new();

        Assembly assembly = Assembly.GetExecutingAssembly();
        const string appSettingsFilePath = "PomodoroTracker.App.appsettings.json";
        using Stream? appSettingsStream = assembly.GetManifestResourceStream(appSettingsFilePath);
        if (appSettingsStream is not null)
        {
            configurationBuilder.AddJsonStream(appSettingsStream);
        }

        IConfigurationRoot configuration = configurationBuilder.Build();
        builder.Configuration.AddConfiguration(configuration);

        builder.Services.Configure<DALOptions>(builder.Configuration.GetSection("PomodoroTracker:DAL"));
        builder.Services.PostConfigure<DALOptions>(options =>
        {
            if (string.IsNullOrWhiteSpace(options.DatabaseDirectory))
            {
                options.DatabaseDirectory = FileSystem.AppDataDirectory;
            }
        });
    }

    private static void RegisterRouting(INavigationService navigationService)
    {
        foreach (RouteModel route in navigationService.Routes)
        {
            Routing.RegisterRoute(route.Route, route.ViewType);
        }
    }

    private static void AssertDALOptionsConfiguration(MauiApp app)
    {
        IOptions<DALOptions> dalOptions = app.Services.GetRequiredService<IOptions<DALOptions>>();

        if (dalOptions?.Value is null)
        {
            throw new InvalidOperationException("No persistence provider configured");
        }

        if (string.IsNullOrEmpty(dalOptions.Value.DatabaseName))
        {
            throw new InvalidOperationException($"{nameof(DALOptions.DatabaseName)} is not set");
        }
    }
}
