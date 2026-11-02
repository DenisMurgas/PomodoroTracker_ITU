// xmurgad00

using Microsoft.Extensions.DependencyInjection;
using PomodoroTracker.BL.Facades;
using PomodoroTracker.BL.Mappers;
using PomodoroTracker.BL.Timer;
using PomodoroTracker.DAL.UnitOfWork;

namespace PomodoroTracker.BL;

public static class BLInstaller
{
    public static IServiceCollection AddBLServices(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();

        services.Scan(selector => selector
            .FromAssemblyOf<BusinessLogic>()
            .AddClasses(filter => filter.AssignableTo(typeof(IFacade<,,>)))
            .AsMatchingInterface()
            .WithSingletonLifetime());

        services.Scan(selector => selector
            .FromAssemblyOf<BusinessLogic>()
            .AddClasses(filter => filter.AssignableTo(typeof(IModelMapper<,,>)))
            .AsSelfWithInterfaces()
            .WithSingletonLifetime());

        services.AddSingleton<IStatisticsFacade, StatisticsFacade>();
        services.AddSingleton<IPomodoroTimerService, PomodoroTimerService>();

        return services;
    }
}
