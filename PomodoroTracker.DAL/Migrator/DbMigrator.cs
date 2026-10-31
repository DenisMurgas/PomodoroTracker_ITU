// Autor: Tým / Spoločné

using PomodoroTracker.DAL.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace PomodoroTracker.DAL.Migrator;

public class DbMigrator(IDbContextFactory<PomodoroTrackerDbContext> dbContextFactory, IOptions<DALOptions> options)
    : IDbMigrator
{
    public void Migrate()
    {
        using PomodoroTrackerDbContext dbContext = dbContextFactory.CreateDbContext();

        if(options.Value.RecreateDatabaseEachTime)
        {
            dbContext.Database.EnsureDeleted();
        }

        dbContext.Database.EnsureCreated();
    }
}
