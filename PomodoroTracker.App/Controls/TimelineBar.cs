// Autor: xmurgad00

using PomodoroTracker.App.Models;

namespace PomodoroTracker.App.Controls;

public class TimelineBar : GraphicsView, IDrawable
{
    public static readonly BindableProperty SegmentsProperty =
        BindableProperty.Create(nameof(Segments), typeof(IEnumerable<TimelineSegmentModel>), typeof(TimelineBar), null, propertyChanged: Redraw);

    public static readonly BindableProperty TrackColorProperty =
        BindableProperty.Create(nameof(TrackColor), typeof(Color), typeof(TimelineBar), Colors.White, propertyChanged: Redraw);

    public static readonly BindableProperty WorkColorProperty =
        BindableProperty.Create(nameof(WorkColor), typeof(Color), typeof(TimelineBar), Colors.Red, propertyChanged: Redraw);

    public static readonly BindableProperty BreakColorProperty =
        BindableProperty.Create(nameof(BreakColor), typeof(Color), typeof(TimelineBar), Colors.Yellow, propertyChanged: Redraw);

    public static readonly BindableProperty OverrunColorProperty =
        BindableProperty.Create(nameof(OverrunColor), typeof(Color), typeof(TimelineBar), Colors.Orange, propertyChanged: Redraw);

    public TimelineBar()
    {
        Drawable = this;
        BackgroundColor = Colors.Transparent;
    }

    public IEnumerable<TimelineSegmentModel>? Segments
    {
        get => (IEnumerable<TimelineSegmentModel>?)GetValue(SegmentsProperty);
        set => SetValue(SegmentsProperty, value);
    }

    public Color TrackColor
    {
        get => (Color)GetValue(TrackColorProperty);
        set => SetValue(TrackColorProperty, value);
    }

    public Color WorkColor
    {
        get => (Color)GetValue(WorkColorProperty);
        set => SetValue(WorkColorProperty, value);
    }

    public Color BreakColor
    {
        get => (Color)GetValue(BreakColorProperty);
        set => SetValue(BreakColorProperty, value);
    }

    public Color OverrunColor
    {
        get => (Color)GetValue(OverrunColorProperty);
        set => SetValue(OverrunColorProperty, value);
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        PathF clip = new();
        clip.AppendRoundedRectangle(dirtyRect, 4);
        canvas.ClipPath(clip);

        canvas.FillColor = TrackColor;
        canvas.FillRectangle(dirtyRect);

        foreach (TimelineSegmentModel segment in Segments ?? [])
        {
            float x = dirtyRect.X + (float)segment.Start * dirtyRect.Width;
            float width = Math.Max(1, (float)(segment.End - segment.Start) * dirtyRect.Width);

            canvas.FillColor = segment.Kind switch
            {
                TimelineSegmentKind.Break => BreakColor,
                TimelineSegmentKind.Overrun => OverrunColor,
                _ => WorkColor
            };
            canvas.FillRectangle(x, dirtyRect.Y, width, dirtyRect.Height);
        }
    }

    private static void Redraw(BindableObject bindable, object oldValue, object newValue)
        => ((TimelineBar)bindable).Invalidate();
}
