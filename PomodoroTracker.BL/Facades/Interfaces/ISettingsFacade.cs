// Autor: xpeterm01

using PomodoroTracker.BL.Models;
using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.BL.Facades;

public interface ISettingsFacade : IFacade<SettingsEntity, SettingsModel, SettingsModel>
{
    Task<SettingsModel> GetCurrentAsync();
}
