// Autor: xmurgad00

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PomodoroTracker.DAL.Factories;
using PomodoroTracker.DAL.Mappers;
using PomodoroTracker.DAL.Migrator;
using PomodoroTracker.DAL.Options;
using PomodoroTracker.DAL.Seeds;

namespace PomodoroTracker.DAL;

public static class DALInstaller
{
    public static IServiceCollection AddDALServices(this IServiceCollection services)
    {
        services.AddSingleton<IDbContextFactory<PomodoroTrackerDbContext>>(serviceProvider =>
        {
            var dalOptions = serviceProvider.GetRequiredService<IOptions<DALOptions>>();
            return new DbContextSqLiteFactory(dalOptions.Value.DatabaseFilePath);
        });
        services.AddSingleton<IDbMigrator, DbMigrator>();
        services.AddSingleton<IDbSeeder, DbSeeder>();

        services.AddSingleton<ProjectEntityMapper>();
        services.AddSingleton<TaskEntityMapper>();
        services.AddSingleton<SessionEntityMapper>();
        services.AddSingleton<TimerProfileEntityMapper>();
        services.AddSingleton<SettingsEntityMapper>();

        return services;
    }
}
