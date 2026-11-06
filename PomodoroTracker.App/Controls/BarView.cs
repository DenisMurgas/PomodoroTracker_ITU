// Autor: xmurgad00

namespace PomodoroTracker.App.Controls;

public class BarView : GraphicsView, IDrawable
{
    private const float ValueTextHeight = 16;

    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(double), typeof(BarView), 0d, propertyChanged: Redraw);

    public static readonly BindableProperty BarColorProperty =
        BindableProperty.Create(nameof(BarColor), typeof(Color), typeof(BarView), Colors.Gray, propertyChanged: Redraw);

    public static readonly BindableProperty TrackColorProperty =
        BindableProperty.Create(nameof(TrackColor), typeof(Color), typeof(BarView), Colors.Transparent, propertyChanged: Redraw);

    public static readonly BindableProperty IsVerticalProperty =
        BindableProperty.Create(nameof(IsVertical), typeof(bool), typeof(BarView), false, propertyChanged: Redraw);

    public static readonly BindableProperty BarCornerRadiusProperty =
        BindableProperty.Create(nameof(BarCornerRadius), typeof(float), typeof(BarView), 4f, propertyChanged: Redraw);

    public static readonly BindableProperty ValueTextProperty =
        BindableProperty.Create(nameof(ValueText), typeof(string), typeof(BarView), string.Empty, propertyChanged: Redraw);

    public static readonly BindableProperty ValueTextColorProperty =
        BindableProperty.Create(nameof(ValueTextColor), typeof(Color), typeof(BarView), Colors.Black, propertyChanged: Redraw);

    public BarView()
    {
        Drawable = this;
        BackgroundColor = Colors.Transparent;
    }

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public Color BarColor
    {
        get => (Color)GetValue(BarColorProperty);
        set => SetValue(BarColorProperty, value);
    }

    public Color TrackColor
    {
        get => (Color)GetValue(TrackColorProperty);
        set => SetValue(TrackColorProperty, value);
    }

    public bool IsVertical
    {
        get => (bool)GetValue(IsVerticalProperty);
        set => SetValue(IsVerticalProperty, value);
    }

    public float BarCornerRadius
    {
        get => (float)GetValue(BarCornerRadiusProperty);
        set => SetValue(BarCornerRadiusProperty, value);
    }

    public string ValueText
    {
        get => (string)GetValue(ValueTextProperty);
        set => SetValue(ValueTextProperty, value);
    }

    public Color ValueTextColor
    {
        get => (Color)GetValue(ValueTextColorProperty);
        set => SetValue(ValueTextColorProperty, value);
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        float value = (float)Math.Clamp(Value, 0, 1);
        float textSpace = IsVertical && !string.IsNullOrEmpty(ValueText) ? ValueTextHeight : 0;
        RectF area = new(dirtyRect.X, dirtyRect.Y + textSpace, dirtyRect.Width, dirtyRect.Height - textSpace);

        if (TrackColor.Alpha > 0)
        {
            canvas.FillColor = TrackColor;
            canvas.FillRoundedRectangle(area, Math.Min(BarCornerRadius, Math.Min(area.Width, area.Height) / 2));
        }

        if (value <= 0)
        {
            return;
        }

        RectF bar = IsVertical
            ? new RectF(area.X, area.Bottom - area.Height * value, area.Width, area.Height * value)
            : new RectF(area.X, area.Y, area.Width * value, area.Height);

        canvas.FillColor = BarColor;
        canvas.FillRoundedRectangle(bar, Math.Min(BarCornerRadius, Math.Min(bar.Width, bar.Height) / 2));

        if (textSpace > 0)
        {
            canvas.FontColor = ValueTextColor;
            canvas.FontSize = 11;
            canvas.Font = Microsoft.Maui.Graphics.Font.DefaultBold;
            canvas.DrawString(ValueText, bar.X, bar.Y - ValueTextHeight, bar.Width, ValueTextHeight - 2,
                HorizontalAlignment.Center, VerticalAlignment.Bottom);
        }
    }

    private static void Redraw(BindableObject bindable, object oldValue, object newValue)
        => ((BarView)bindable).Invalidate();
}
