// Autor: xpeterm01

using Microsoft.EntityFrameworkCore;

namespace PomodoroTracker.DAL.Factories;

public class DbContextSqLiteFactory : IDbContextFactory<PomodoroTrackerDbContext>
{
    private readonly DbContextOptionsBuilder<PomodoroTrackerDbContext> _contextOptionsBuilder = new();

    public DbContextSqLiteFactory(string databaseName)
    {
        _contextOptionsBuilder.UseSqlite($"Data Source={databaseName}");
    }

    public PomodoroTrackerDbContext CreateDbContext() => new(_contextOptionsBuilder.Options);
}
