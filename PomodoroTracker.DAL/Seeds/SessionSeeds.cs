// Autor: Tým / Spoločné

using PomodoroTracker.Common.Enums;
using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.DAL.Seeds;

public static class SessionSeeds
{
    private static readonly DateTime Today = DateTime.Today;

    private static DateTime At(int hour, int minute) => Today.AddHours(hour).AddMinutes(minute);

    public static readonly SessionEntity Work1 = new()
    {
        Id = Guid.Parse("2ed04dcc-1818-4319-88e9-6877f2fca915"),
        Type = SessionType.Work,
        StartedAt = At(8, 40),
        EndedAt = At(9, 30),
        PlannedDuration = TimeSpan.FromMinutes(50),
        TaskId = TaskSeeds.LoginRefactor.Id
    };

    public static readonly SessionEntity Break1 = new()
    {
        Id = Guid.Parse("7a6b0669-54cb-4bb9-bf3b-5bc5395531c1"),
        Type = SessionType.ShortBreak,
        StartedAt = At(9, 30),
        EndedAt = At(9, 40),
        PlannedDuration = TimeSpan.FromMinutes(10)
    };

    public static readonly SessionEntity Work2 = new()
    {
        Id = Guid.Parse("21a030b8-b7c6-4ce4-8019-06f7461ac1df"),
        Type = SessionType.Work,
        StartedAt = At(9, 40),
        EndedAt = At(10, 30),
        PlannedDuration = TimeSpan.FromMinutes(50),
        TaskId = TaskSeeds.LoginRefactor.Id
    };

    public static readonly SessionEntity Break2Overrun = new()
    {
        Id = Guid.Parse("e426f807-73d3-4a42-9096-3015429d03f6"),
        Type = SessionType.ShortBreak,
        StartedAt = At(10, 30),
        EndedAt = At(10, 52),
        PlannedDuration = TimeSpan.FromMinutes(10)
    };

    public static readonly SessionEntity Work3 = new()
    {
        Id = Guid.Parse("bea8178d-47c0-40b8-bff8-08344fb1cfa6"),
        Type = SessionType.Work,
        StartedAt = At(10, 52),
        EndedAt = At(11, 42),
        PlannedDuration = TimeSpan.FromMinutes(50),
        TaskId = TaskSeeds.Wireframes.Id
    };

    public static readonly SessionEntity Work4 = new()
    {
        Id = Guid.Parse("e415b9f2-ac74-4d02-8cfc-a9eedc9c0632"),
        Type = SessionType.Work,
        StartedAt = At(12, 30),
        EndedAt = At(13, 20),
        PlannedDuration = TimeSpan.FromMinutes(50),
        TaskId = TaskSeeds.EntityDiagram.Id
    };

    public static readonly SessionEntity Break3 = new()
    {
        Id = Guid.Parse("801721e6-e455-498a-bd9e-f9543d9f0592"),
        Type = SessionType.ShortBreak,
        StartedAt = At(13, 20),
        EndedAt = At(13, 30),
        PlannedDuration = TimeSpan.FromMinutes(10)
    };

    public static readonly SessionEntity Work5 = new()
    {
        Id = Guid.Parse("d6c7b1a2-3e4f-4a5b-9c8d-7e6f5a4b3c2d"),
        Type = SessionType.Work,
        StartedAt = At(13, 30),
        EndedAt = At(14, 20),
        PlannedDuration = TimeSpan.FromMinutes(50),
        TaskId = TaskSeeds.CodeReview.Id
    };

    private static readonly TimeSpan WorkBlock = TimeSpan.FromMinutes(50);
    private static readonly TimeSpan ShortBreak = TimeSpan.FromMinutes(10);

    private static readonly (int DaysAgo, int StartHour, int Blocks, Guid TaskId)[] HistoryPlan =
    [
        (1, 9, 3, TaskSeeds.LoginRefactor.Id),
        (1, 19, 1, TaskSeeds.GuitarScales.Id),
        (2, 8, 5, TaskSeeds.Wireframes.Id),
        (3, 10, 2, TaskSeeds.EntityDiagram.Id),
        (4, 9, 3, TaskSeeds.CodeReview.Id),
        (4, 18, 1, TaskSeeds.GuitarScales.Id),
        (5, 13, 4, TaskSeeds.ApiTests.Id),
        (6, 10, 2, TaskSeeds.LoginRefactor.Id)
    ];

    public static readonly IReadOnlyList<SessionEntity> History = BuildHistory();

    public static IReadOnlyList<SessionEntity> All =>
        [Work1, Break1, Work2, Break2Overrun, Work3, Work4, Break3, Work5, .. History];

    private static List<SessionEntity> BuildHistory()
    {
        List<SessionEntity> sessions = [];
        int number = 0;

        foreach ((int daysAgo, int startHour, int blocks, Guid taskId) in HistoryPlan)
        {
            DateTime start = Today.AddDays(-daysAgo).AddHours(startHour);

            for (int block = 0; block < blocks; block++)
            {
                sessions.Add(new SessionEntity
                {
                    Id = HistoryId(++number),
                    Type = SessionType.Work,
                    StartedAt = start,
                    EndedAt = start + WorkBlock,
                    PlannedDuration = WorkBlock,
                    TaskId = taskId
                });
                start += WorkBlock;

                if (block < blocks - 1)
                {
                    sessions.Add(new SessionEntity
                    {
                        Id = HistoryId(++number),
                        Type = SessionType.ShortBreak,
                        StartedAt = start,
                        EndedAt = start + ShortBreak,
                        PlannedDuration = ShortBreak
                    });
                    start += ShortBreak;
                }
            }
        }

        return sessions;
    }

    private static Guid HistoryId(int number)
        => Guid.Parse($"00000000-0000-4000-8000-{number:000000000000}");
}
