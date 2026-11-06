// Autor: xmurgad00

using PomodoroTracker.BL.Models;

namespace PomodoroTracker.App.Models;

public record TaskRowModel(TaskListModel Item, bool IsLastWorked);
