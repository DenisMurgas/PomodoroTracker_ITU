// Autor: Tým / Spoločné

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PomodoroTracker.DAL.Factories;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PomodoroTrackerDbContext>
{
    private readonly DbContextSqLiteFactory _dbContextSqLiteFactory = new("pomodorotracker.db");

    public PomodoroTrackerDbContext CreateDbContext(string[] args) => _dbContextSqLiteFactory.CreateDbContext();
}
