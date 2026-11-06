// Autor: xmurgad00

using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace PomodoroTracker.App.Converters;

public class HexToSoftColorConverter : BaseConverterOneWay<string, Color>
{
    private const float SoftAlpha = 0.16f;

    public override Color DefaultConvertReturnValue { get; set; } = Colors.Transparent;

    public override Color ConvertFrom(string value, CultureInfo? culture)
        => Color.TryParse(value, out Color color) ? color.WithAlpha(SoftAlpha) : DefaultConvertReturnValue;
}
