// Autor: xmurgad00

using CommunityToolkit.Mvvm.ComponentModel;

namespace PomodoroTracker.App.Models;

public partial class ProjectFilterModel(Guid? projectId, string name, string color) : ObservableObject
{
    public Guid? ProjectId { get; } = projectId;
    public string Name { get; } = name;
    public string Color { get; } = color;
    public bool HasColor => !string.IsNullOrEmpty(Color);

    [ObservableProperty]
    public partial bool IsSelected { get; set; }
}
