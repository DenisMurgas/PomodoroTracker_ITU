// Autor: Tým / Spoločné

using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.DAL.Seeds;

public static class SettingsSeeds
{
    public static readonly SettingsEntity Default = new()
    {
        Id = Guid.Parse("3f9c672d-8b0f-44e2-b557-ef7712f1f5b2"),
        AutoStartNext = true,
        NotificationsEnabled = true,
        ActiveTimerProfileId = TimerProfileSeeds.Focus.Id
    };
}
