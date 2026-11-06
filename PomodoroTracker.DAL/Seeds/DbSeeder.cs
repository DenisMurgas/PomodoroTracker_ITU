// Autor: Tým / Spoločné

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PomodoroTracker.DAL.Entities;
using PomodoroTracker.DAL.Options;

namespace PomodoroTracker.DAL.Seeds;

public class DbSeeder(IDbContextFactory<PomodoroTrackerDbContext> dbContextFactory, IOptions<DALOptions> options)
    : IDbSeeder
{
    public void Seed()
    {
        using PomodoroTrackerDbContext dbContext = dbContextFactory.CreateDbContext();

        SeedDefaults(dbContext);

        if (options.Value.SeedDemoData)
        {
            SeedDemoData(dbContext);
        }

        dbContext.SaveChanges();
    }

    private static void SeedDefaults(PomodoroTrackerDbContext dbContext)
    {
        foreach (TimerProfileEntity profile in TimerProfileSeeds.All)
        {
            AddIfMissing(dbContext, profile with { });
        }

        AddIfMissing(dbContext, SettingsSeeds.Default with { ActiveTimerProfile = null });
    }

    private static void SeedDemoData(PomodoroTrackerDbContext dbContext)
    {
        foreach (ProjectEntity project in ProjectSeeds.All)
        {
            AddIfMissing(dbContext, project with { Tasks = [] });
        }

        foreach (TaskEntity task in TaskSeeds.All)
        {
            AddIfMissing(dbContext, task with { Project = null, Sessions = [] });
        }

        foreach (SessionEntity session in SessionSeeds.All)
        {
            AddIfMissing(dbContext, session with { Task = null });
        }
    }

    private static void AddIfMissing<TEntity>(DbContext dbContext, TEntity entity)
        where TEntity : class, IEntity
    {
        if (!dbContext.Set<TEntity>().Any(e => e.Id == entity.Id))
        {
            dbContext.Set<TEntity>().Add(entity);
        }
    }
}
