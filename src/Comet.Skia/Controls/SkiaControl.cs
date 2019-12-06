using System;
using System.Drawing;
using Comet.Skia.Internal;
using SkiaSharp;
using TextBlock = Topten.RichTextKit.TextBlock;

namespace Comet.Skia
{
	public abstract class SkiaControl : SkiaView
	{
		public static DrawMapper<View> DrawMapper = new DrawMapper<View>()
		{
			[SkiaEnvironmentKeys.Background] = DrawBackground,
			[SkiaEnvironmentKeys.Border] = DrawBorder,
			[SkiaEnvironmentKeys.Text] = DrawText,
			[SkiaEnvironmentKeys.Overlay] = DrawOverlay,
			[SkiaEnvironmentKeys.Clip] = ClipCanvas,
		};

		public static PropertyMapper<View> Mapper = new PropertyMapper<View>
		{
			[EnvironmentKeys.Colors.BackgroundColor] = Redraw,
			[EnvironmentKeys.View.Border] = Redraw,
			[EnvironmentKeys.View.Shadow] = Redraw,
			[EnvironmentKeys.View.ClipShape] = Redraw,
			[EnvironmentKeys.View.Overlay] = Redraw,
			[EnvironmentKeys.Text.Alignment] = MapResetText,
			[EnvironmentKeys.Fonts.Family] = MapResetText,
			[EnvironmentKeys.Fonts.Italic] = MapResetText,
			[EnvironmentKeys.Fonts.Size] = MapResetText,
			[EnvironmentKeys.Fonts.Weight] = MapResetText,
		};

		protected SkiaControl() : base() { }

		public View VirtualView { get; private set; }

		public override SizeF Measure(SizeF availableSize) => new SizeF(100, 44);

		public virtual void SetView(View view)
		{
			VirtualView = view;
			this.Parent = view?.Parent;
		}


		protected virtual void DrawBorder(SKCanvas canvas, Shape shape, RectangleF rect)
		{
			var strokeColor = shape.GetStrokeColor(VirtualView, Color.Black);
			var strokeWidth = shape.GetLineWidth(VirtualView, 1);
			var fill = shape.GetFill(VirtualView, Color.Transparent);
			canvas.DrawShape(shape, rect, strokeColor: strokeColor, strokeWidth: strokeWidth, fill: fill, drawingStyle: Graphics.DrawingStyle.StrokeFill);
		}

		protected virtual void DrawOverlay(SKCanvas canvas, Shape shape, RectangleF rect)
		{
			var strokeColor = shape.GetStrokeColor(VirtualView, Color.Black);
			var strokeWidth = shape.GetLineWidth(VirtualView, 1);
			var fill = shape.GetFill(VirtualView,Color.Transparent);
			canvas.DrawShape(shape, rect, strokeColor: strokeColor, strokeWidth: strokeWidth, fill: fill, drawingStyle:Graphics.DrawingStyle.StrokeFill);
		}
		protected virtual void DrawBackground(SKCanvas canvas, Color backgroundColor)
		{
			if (backgroundColor == null)
				return;
			var paint = new SKPaint();
			paint.Color = backgroundColor.ToSKColor();
			var bounds = new SKRect(0, 0, VirtualView.Frame.Width, VirtualView.Frame.Height);
			canvas.DrawRect(bounds, paint);
		}

		protected void DrawText(string text, SKCanvas canvas, FontAttributes data, Color color, TextAlignment alignment, LineBreakMode lineBreakMode, VerticalAlignment verticalAlignment)
		{

			var tb = new TextBlock();
			tb.AddText(text, data.ToStyle(color));
			tb.MaxWidth = VirtualView.Frame.Width;
			tb.MaxHeight = VirtualView.Frame.Height;
			tb.MaxLines = null;
			tb.Alignment = alignment.ToTextAlignment();
			tb.Layout();

			var y = verticalAlignment switch
			{
				VerticalAlignment.Bottom => VirtualView.Frame.Height - tb.MeasuredHeight,
				VerticalAlignment.Center => (VirtualView.Frame.Height - tb.MeasuredHeight) / 2,
				_ => 0
			};

			tb.Paint(canvas, new SKPoint(0, y));
		}

		protected void DrawText(TextBlock tb, SKCanvas canvas, VerticalAlignment verticalAlignment)
		{
			tb.MaxWidth = VirtualView.Frame.Width;
			tb.MaxHeight = VirtualView.Frame.Height;
			tb.Layout();
			var y = verticalAlignment switch
			{
				VerticalAlignment.Bottom => VirtualView.Frame.Height - tb.MeasuredHeight,
				VerticalAlignment.Center => (VirtualView.Frame.Height - tb.MeasuredHeight) / 2,
				_ => 0
			};

			tb.Paint(canvas, new SKPoint(0, y));
		}

		public abstract string AccessibilityText();

		public static void ClipCanvas(SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, View view)
		{
			var border = view?.GetBorder();
			var clipShape = view?.GetClipShape() ?? border;
            if (clipShape != null)
                canvas.ClipPath(clipShape.PathForBounds(dirtyRect).ToSKPath());
        }

		public static void DrawBackground(SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, View view)
		{
			control?.DrawBackground(canvas, view.GetBackgroundColor(Color.Transparent));
		}

		public static void DrawBorder(SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, View view)
		{
			var shape = view.GetBorder();
			if (shape == null)
				return;
			control?.DrawBorder(canvas, shape, dirtyRect);
		}

		public static void DrawText(SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, View view)
		{
			var textHandler = control as ITextHandler;
			if (textHandler == null)
				return;
			if (textHandler.TextBlock == null)
				textHandler.TextBlock = textHandler.CreateTextBlock();
			control?.DrawText(textHandler.TextBlock, canvas, textHandler.VerticalAlignment);
		}

		public static void DrawOverlay(SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, View view)
		{
			var shape = view.GetOverlay();
			if (shape == null)
				return;
			control?.DrawOverlay(canvas, shape, dirtyRect);
		}

		public static void Redraw(IViewHandler viewHandler, View virtualView)
		{
			var control = viewHandler as SkiaControl;
			control.Invalidate();
		}

		public static void MapResetText(IViewHandler viewHandler, View virtualView)
		{
			var textHandler = viewHandler as ITextHandler;
			textHandler.TextBlock = null;

			////nativeView.SetTitle(virtualView.Text?.CurrentValue, UIControlState.Normal);
			virtualView.InvalidateMeasurement();
		}
		//public static void MapColorProperty(IViewHandler viewHandler, Button virtualView)
		//{
		//	var nativeView = (UIButton)viewHandler.NativeView;
		//	nativeView.SetTitleColor(virtualView.GetColor(DefaultColor).ToUIColor(), UIControlState.Normal);
		//}
	}
}
