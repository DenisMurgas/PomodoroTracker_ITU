// Autor: Tým / Spoločné

using PomodoroTracker.App.Services;

namespace PomodoroTracker.App.ViewModels;

public partial class StatisticsViewModel(IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
}
