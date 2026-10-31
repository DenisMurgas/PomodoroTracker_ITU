// Autor: xmurgad00

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PomodoroTracker.DAL.UnitOfWork;

public class UnitOfWorkFactory(
    IDbContextFactory<PomodoroTrackerDbContext> dbContextFactory,
    IServiceScopeFactory serviceScopeFactory) : IUnitOfWorkFactory
{
    public IUnitOfWork Create()
    {
        var scope = serviceScopeFactory.CreateScope();
        return new UnitOfWork(dbContextFactory.CreateDbContext(), scope);
    }
}
