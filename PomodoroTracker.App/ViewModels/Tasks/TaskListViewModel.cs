// Autor: Tým / Spoločné

using PomodoroTracker.App.Services;

namespace PomodoroTracker.App.ViewModels;

public partial class TaskListViewModel(IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
}
