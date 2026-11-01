// Autor: xmurgad00

using PomodoroTracker.BL.Models;
using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.BL.Facades;

public interface IProjectFacade : IFacade<ProjectEntity, ProjectListModel, ProjectDetailModel>
{
}
