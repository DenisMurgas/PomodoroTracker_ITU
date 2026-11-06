// Autor: xmurgad00

using CommunityToolkit.Mvvm.Messaging;

namespace PomodoroTracker.App.Services;

public interface IMessengerService
{
    IMessenger Messenger { get; }

    void Send<TMessage>(TMessage message)
        where TMessage : class;
}
