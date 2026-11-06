// Autor: Tým / Spoločné

using System.Windows.Input;

namespace PomodoroTracker.App.Controls;

public partial class NavigationRailItem : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(NavigationRailItem), string.Empty);

    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(NavigationRailItem));

    public static readonly BindableProperty RouteProperty =
        BindableProperty.Create(nameof(Route), typeof(string), typeof(NavigationRailItem), string.Empty,
            propertyChanged: OnRouteChanged);

    public static readonly BindableProperty CurrentRouteProperty =
        BindableProperty.Create(nameof(CurrentRoute), typeof(string), typeof(NavigationRailItem), string.Empty,
            propertyChanged: OnRouteChanged);

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(NavigationRailItem));

    public static readonly BindableProperty IsSelectedProperty =
        BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(NavigationRailItem), false);

    public NavigationRailItem()
    {
        InitializeComponent();
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public ImageSource? Icon
    {
        get => (ImageSource?)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string Route
    {
        get => (string)GetValue(RouteProperty);
        set => SetValue(RouteProperty, value);
    }

    public string CurrentRoute
    {
        get => (string)GetValue(CurrentRouteProperty);
        set => SetValue(CurrentRouteProperty, value);
    }

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        private set => SetValue(IsSelectedProperty, value);
    }

    private static void OnRouteChanged(BindableObject bindable, object oldValue, object newValue)
    {
        NavigationRailItem item = (NavigationRailItem)bindable;
        item.IsSelected = !string.IsNullOrEmpty(item.Route)
                          && string.Equals(item.Route, item.CurrentRoute, StringComparison.OrdinalIgnoreCase);
    }
}
