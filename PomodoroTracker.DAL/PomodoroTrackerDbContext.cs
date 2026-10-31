// Autor: xpeterm01

using Microsoft.EntityFrameworkCore;
using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.DAL;

public class PomodoroTrackerDbContext(DbContextOptions contextOptions) : DbContext(contextOptions)
{
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
    public DbSet<SessionEntity> Sessions => Set<SessionEntity>();
    public DbSet<TimerProfileEntity> TimerProfiles => Set<TimerProfileEntity>();
    public DbSet<SettingsEntity> Settings => Set<SettingsEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PomodoroTrackerDbContext).Assembly);
    }
}
