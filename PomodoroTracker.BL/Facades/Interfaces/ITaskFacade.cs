// Autor: xmurgad00

using PomodoroTracker.BL.Models;
using PomodoroTracker.DAL.Entities;

namespace PomodoroTracker.BL.Facades;

public interface ITaskFacade : IFacade<TaskEntity, TaskListModel, TaskDetailModel>
{
    Task<IEnumerable<TaskListModel>> GetByProjectAsync(Guid projectId);
    Task SetCompletedAsync(Guid taskId, bool isCompleted);
    Task<TaskListModel?> GetLastWorkedAsync();
}
