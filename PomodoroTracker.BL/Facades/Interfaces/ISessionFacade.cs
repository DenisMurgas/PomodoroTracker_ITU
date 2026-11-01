// Autor: xpeterm01

using PomodoroTracker.BL.Models;
using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.BL.Facades;

public interface ISessionFacade : IFacade<SessionEntity, SessionListModel, SessionDetailModel>
{
    Task<IEnumerable<SessionListModel>> GetByRangeAsync(DateTime from, DateTime to);
    Task<IEnumerable<SessionDetailModel>> GetUnfinishedAsync();
}
