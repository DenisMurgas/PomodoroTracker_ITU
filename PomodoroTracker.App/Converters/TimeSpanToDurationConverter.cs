// Autor: Tým / Spoločné

using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace PomodoroTracker.App.Converters;

public class TimeSpanToDurationConverter : BaseConverterOneWay<TimeSpan, string>
{
    public override string DefaultConvertReturnValue { get; set; } = "0 min";

    public override string ConvertFrom(TimeSpan value, CultureInfo? culture)
    {
        int totalMinutes = (int)value.Duration().TotalMinutes;
        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;

        if (totalMinutes == 0 && value.Duration().Seconds > 0)
        {
            return $"{value.Duration().Seconds} s";
        }

        return hours > 0
            ? $"{hours} h {minutes:00} min"
            : $"{minutes} min";
    }
}
