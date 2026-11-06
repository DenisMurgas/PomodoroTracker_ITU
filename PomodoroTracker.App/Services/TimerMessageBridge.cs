// Autor: xmurgad00

using PomodoroTracker.App.Messages;
using PomodoroTracker.BL.Models;
using PomodoroTracker.BL.Timer;
using PomodoroTracker.Common.Enums;

namespace PomodoroTracker.App.Services;

public class TimerMessageBridge(IPomodoroTimerService timerService, IMessengerService messengerService)
    : ITimerMessageBridge
{
    private bool _isStarted;

    public async Task StartAsync()
    {
        if (_isStarted)
        {
            return;
        }

        _isStarted = true;

        timerService.SessionFinished += OnSessionFinished;
        timerService.PlannedTimeElapsed += OnPlannedTimeElapsed;

        await timerService.InitializeAsync();
    }

    private void OnSessionFinished(object? sender, SessionDetailModel session)
        => messengerService.Send(new SessionFinishedMessage { Session = session });

    private void OnPlannedTimeElapsed(object? sender, SessionType phase)
        => messengerService.Send(new PlannedTimeElapsedMessage { Phase = phase });
}
