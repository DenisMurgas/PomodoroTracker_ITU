// Autor: Tým / Spoločné

using PomodoroTracker.App.Services;
using PomodoroTracker.App.Shells;

namespace PomodoroTracker.App;

public partial class App : Application
{
    private const string WindowTitle = "Pomodoro tracker";
    private const double WindowWidth = 736;
    private const double WindowHeight = 460;
    private const double MaximumWindowWidth = 1024;
    private const double MaximumWindowHeight = 640;

    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();

        UserAppTheme = AppTheme.Light;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        AppShell shell = _serviceProvider.GetRequiredService<AppShell>();

        return new Window(shell)
        {
            Title = WindowTitle,
            Width = WindowWidth,
            Height = WindowHeight,
            MinimumWidth = WindowWidth,
            MinimumHeight = WindowHeight,
            MaximumWidth = MaximumWindowWidth,
            MaximumHeight = MaximumWindowHeight,
            TitleBar = new TitleBar
            {
                Title = WindowTitle,
                BackgroundColor = GetColor("BackgroundMainColor"),
                ForegroundColor = GetColor("TextPrimaryColor")
            }
        };
    }

    protected override async void OnStart()
    {
        base.OnStart();

        await _serviceProvider.GetRequiredService<ITimerMessageBridge>().StartAsync();
    }

    private Color GetColor(string key)
        => Resources.TryGetValue(key, out object? value) && value is Color color ? color : Colors.White;
}
