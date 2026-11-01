// Autor: Tým / Spoločné

using PomodoroTracker.Common.Enums;
using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.BL.Calculations;

public static class SessionCalculations
{
    public static TimeSpan ActualDuration(DateTime startedAt, DateTime? endedAt, TimeSpan pausedDuration)
    {
        if (endedAt is null)
        {
            return TimeSpan.Zero;
        }

        TimeSpan duration = endedAt.Value - startedAt - pausedDuration;
        return duration < TimeSpan.Zero ? TimeSpan.Zero : duration;
    }

    public static TimeSpan ActualDuration(SessionEntity session)
        => ActualDuration(session.StartedAt, session.EndedAt, session.PausedDuration);

    public static TimeSpan Overrun(SessionType type, TimeSpan actualDuration, TimeSpan plannedDuration)
    {
        if (type == SessionType.Work || actualDuration <= plannedDuration)
        {
            return TimeSpan.Zero;
        }

        return actualDuration - plannedDuration;
    }

    public static TimeSpan Overrun(SessionEntity session)
        => Overrun(session.Type, ActualDuration(session), session.PlannedDuration);

    public static bool IsFinishedWork(SessionEntity session)
        => session.Type == SessionType.Work && session.EndedAt is not null;

    public static TimeSpan WorkTime(IEnumerable<SessionEntity> sessions)
        => Sum(sessions.Where(IsFinishedWork).Select(ActualDuration));

    public static TimeSpan Sum(IEnumerable<TimeSpan> values)
        => values.Aggregate(TimeSpan.Zero, (total, value) => total + value);
}
