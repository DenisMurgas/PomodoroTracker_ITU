// Autor: Tým / Spoločné

using CommunityToolkit.Mvvm.ComponentModel;

namespace PomodoroTracker.BL.Models;

public abstract class ModelBase : ObservableObject
{
    public Guid Id { get; set; }
}
