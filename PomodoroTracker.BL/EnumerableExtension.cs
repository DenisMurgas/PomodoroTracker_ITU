// Autor: Tým / Spoločné

using System.Collections.ObjectModel;

namespace PomodoroTracker.BL;

public static class EnumerableExtension
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> values)
        => new(values);
}
