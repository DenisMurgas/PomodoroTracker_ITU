// Autor: xpeterm01

namespace PomodoroTracker.App.Controls;

public class ProgressRing : GraphicsView, IDrawable
{
    public static readonly BindableProperty ProgressProperty =
        BindableProperty.Create(nameof(Progress), typeof(double), typeof(ProgressRing), 0d, propertyChanged: Redraw);

    public static readonly BindableProperty TrackColorProperty =
        BindableProperty.Create(nameof(TrackColor), typeof(Color), typeof(ProgressRing), Colors.White, propertyChanged: Redraw);

    public static readonly BindableProperty ProgressColorProperty =
        BindableProperty.Create(nameof(ProgressColor), typeof(Color), typeof(ProgressRing), Colors.Red, propertyChanged: Redraw);

    public static readonly BindableProperty ThicknessProperty =
        BindableProperty.Create(nameof(Thickness), typeof(float), typeof(ProgressRing), 12f, propertyChanged: Redraw);

    public ProgressRing()
    {
        Drawable = this;
        BackgroundColor = Colors.Transparent;
    }

    public double Progress
    {
        get => (double)GetValue(ProgressProperty);
        set => SetValue(ProgressProperty, value);
    }

    public Color TrackColor
    {
        get => (Color)GetValue(TrackColorProperty);
        set => SetValue(TrackColorProperty, value);
    }

    public Color ProgressColor
    {
        get => (Color)GetValue(ProgressColorProperty);
        set => SetValue(ProgressColorProperty, value);
    }

    public float Thickness
    {
        get => (float)GetValue(ThicknessProperty);
        set => SetValue(ThicknessProperty, value);
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        float size = Math.Min(dirtyRect.Width, dirtyRect.Height) - Thickness;
        if (size <= 0)
        {
            return;
        }

        float x = dirtyRect.Center.X - size / 2;
        float y = dirtyRect.Center.Y - size / 2;

        canvas.StrokeSize = Thickness;
        canvas.StrokeColor = TrackColor;
        canvas.DrawEllipse(x, y, size, size);

        float progress = (float)Math.Clamp(Progress, 0, 1);
        if (progress <= 0)
        {
            return;
        }

        canvas.StrokeColor = ProgressColor;

        if (progress >= 1)
        {
            canvas.DrawEllipse(x, y, size, size);
            return;
        }

        canvas.StrokeLineCap = LineCap.Round;
        canvas.DrawArc(x, y, size, size, 90, 90 - 360 * progress, true, false);
    }

    private static void Redraw(BindableObject bindable, object oldValue, object newValue)
        => ((ProgressRing)bindable).Invalidate();
}
