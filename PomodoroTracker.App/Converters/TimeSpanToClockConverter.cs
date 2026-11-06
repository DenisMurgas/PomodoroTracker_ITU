// Autor: Tým / Spoločné

using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace PomodoroTracker.App.Converters;

public class TimeSpanToClockConverter : BaseConverterOneWay<TimeSpan, string>
{
    public override string DefaultConvertReturnValue { get; set; } = "00:00";

    public override string ConvertFrom(TimeSpan value, CultureInfo? culture)
    {
        TimeSpan time = value.Duration();
        return $"{(int)time.TotalMinutes:00}:{time.Seconds:00}";
    }
}
