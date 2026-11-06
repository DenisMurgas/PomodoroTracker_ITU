// Autor: Tým / Spoločné

using PomodoroTracker.App.Services;

namespace PomodoroTracker.App.ViewModels;

public partial class TimerViewModel(IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
}
