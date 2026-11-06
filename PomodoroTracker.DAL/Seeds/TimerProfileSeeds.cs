// Autor: Tým / Spoločné

using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.DAL.Seeds;

public static class TimerProfileSeeds
{
    public static readonly TimerProfileEntity Classic = new()
    {
        Id = Guid.Parse("f481106b-2f32-4fc7-ad0e-e3cd9c171b2b"),
        Name = "25 / 5",
        WorkDuration = TimeSpan.FromMinutes(25),
        ShortBreakDuration = TimeSpan.FromMinutes(5),
        LongBreakDuration = TimeSpan.FromMinutes(15),
        LongBreakInterval = 4
    };

    public static readonly TimerProfileEntity Focus = new()
    {
        Id = Guid.Parse("0cc5b7c8-cffb-4cc1-9fc8-cf717500c236"),
        Name = "50 / 10",
        WorkDuration = TimeSpan.FromMinutes(50),
        ShortBreakDuration = TimeSpan.FromMinutes(10),
        LongBreakDuration = TimeSpan.FromMinutes(20),
        LongBreakInterval = 4
    };

    public static readonly TimerProfileEntity DeepWork = new()
    {
        Id = Guid.Parse("d2ef5f56-f5ac-4c6b-af28-c76161d78794"),
        Name = "90 / 15",
        WorkDuration = TimeSpan.FromMinutes(90),
        ShortBreakDuration = TimeSpan.FromMinutes(15),
        LongBreakDuration = TimeSpan.FromMinutes(30),
        LongBreakInterval = 3
    };

    public static IReadOnlyList<TimerProfileEntity> All => [Classic, Focus, DeepWork];
}
