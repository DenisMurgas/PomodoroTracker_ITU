// Autor: Tým / Spoločné

using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace PomodoroTracker.App.Converters;

public class HexToColorConverter : BaseConverterOneWay<string, Color>
{
    public override Color DefaultConvertReturnValue { get; set; } = Colors.Gray;

    public override Color ConvertFrom(string value, CultureInfo? culture)
        => Color.TryParse(value, out Color color) ? color : DefaultConvertReturnValue;
}
