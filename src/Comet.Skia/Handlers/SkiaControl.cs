using System;
using System.Drawing;
using Comet.Skia.Internal;
using SkiaSharp;
using TextBlock = Topten.RichTextKit.TextBlock;

namespace Comet.Skia {
	public abstract class SkiaControl : SkiaView {
		public static readonly DrawMapper<View> DrawMapper = new DrawMapper<View> () {
			[SkiaEnvironmentKeys.Background] = DrawBackground,
			[SkiaEnvironmentKeys.Border] = DrawBorder,
			[SkiaEnvironmentKeys.Text] = DrawText,
			[SkiaEnvironmentKeys.Overlay] = DrawOverlay,
			[SkiaEnvironmentKeys.Clip] = ClipCanvas,
		};

		public new static readonly PropertyMapper<View> Mapper = new PropertyMapper<View> {
			[EnvironmentKeys.Colors.BackgroundColor] = MapBackgroundColor,
			[EnvironmentKeys.Colors.Color] = MapBackgroundColor,
			[EnvironmentKeys.View.Border] = MapBorderProperty,
			[EnvironmentKeys.View.Shadow] = MapShadowProperty,
			[EnvironmentKeys.View.ClipShape] = MapClipShapeProperty,
			[EnvironmentKeys.View.Overlay] = MapOverlayProperty,
			[EnvironmentKeys.Text.Alignment] = MapResetText,
			[EnvironmentKeys.Fonts.Family] = MapResetText,
			[EnvironmentKeys.Fonts.Italic] = MapResetText,
			[EnvironmentKeys.Fonts.Size] = MapResetText,
			[EnvironmentKeys.Fonts.Weight] = MapResetText,
		};

		public static string [] DefaultLayerDrawingOrder = new []
		{
			SkiaEnvironmentKeys.Clip,
			SkiaEnvironmentKeys.Background,
			SkiaEnvironmentKeys.Border,
			SkiaEnvironmentKeys.Text,
			SkiaEnvironmentKeys.Overlay,
		};

		protected SkiaControl () : base () { }

		public View VirtualView { get; private set; }

		public override SizeF GetIntrinsicSize (SizeF availableSize) => new SizeF (100, 44);

		public virtual void SetView (View view)
		{
			VirtualView = view;
			this.Parent = view?.Parent;
		}


		protected virtual void DrawBorder (SKCanvas canvas, Shape shape, RectangleF rect)
		{
			var strokeColor = shape.GetStrokeColor (VirtualView, Color.Black);
			var strokeWidth = shape.GetLineWidth (VirtualView, 1);
			var fill = shape.GetFill (VirtualView, Color.Transparent);
			canvas.DrawShape (shape, rect, strokeColor: strokeColor, strokeWidth: strokeWidth, fill: fill, drawingStyle: Graphics.DrawingStyle.StrokeFill);
		}

		protected virtual void DrawOverlay (SKCanvas canvas, Shape shape, RectangleF rect)
		{
			var strokeColor = shape.GetStrokeColor (VirtualView, Color.Black);
			var strokeWidth = shape.GetLineWidth (VirtualView, 1);
			var fill = shape.GetFill (VirtualView, Color.Transparent);
			canvas.DrawShape (shape, rect, strokeColor: strokeColor, strokeWidth: strokeWidth, fill: fill, drawingStyle: Graphics.DrawingStyle.StrokeFill);
		}
		protected virtual void DrawBackground (SKCanvas canvas, Color backgroundColor, RectangleF rect)
		{
			if (backgroundColor == null)
				return;
			var paint = new SKPaint ();
			paint.Color = backgroundColor.ToSKColor ();
			canvas.DrawRect (rect.ToSKRect (), paint);
		}

		protected virtual void DrawText (string text, SKCanvas canvas, FontAttributes data, Color color, TextAlignment alignment, LineBreakMode lineBreakMode, VerticalAlignment verticalAlignment)
		{

			var tb = new TextBlock ();
			tb.AddText (text, data.ToStyle (color));
			tb.MaxWidth = VirtualView.Frame.Width;
			tb.MaxHeight = VirtualView.Frame.Height;
			tb.MaxLines = null;
			tb.Alignment = alignment.ToTextAlignment ();
			tb.Layout ();

			var y = verticalAlignment switch
			{
				VerticalAlignment.Bottom => VirtualView.Frame.Height - tb.MeasuredHeight,
				VerticalAlignment.Center => (VirtualView.Frame.Height - tb.MeasuredHeight) / 2,
				_ => 0
			};

			tb.Paint (canvas, new SKPoint (0, y));
		}

		protected virtual void DrawText (TextBlock tb, SKCanvas canvas, VerticalAlignment verticalAlignment)
		{
			tb.MaxWidth = VirtualView.Frame.Width;
			tb.MaxHeight = VirtualView.Frame.Height;
			tb.Layout ();
			var y = verticalAlignment switch
			{
				VerticalAlignment.Bottom => VirtualView.Frame.Height - tb.MeasuredHeight,
				VerticalAlignment.Center => (VirtualView.Frame.Height - tb.MeasuredHeight) / 2,
				_ => 0
			};

			tb.Paint (canvas, new SKPoint (0, y));
		}

		public abstract string AccessibilityText ();

		public static void ClipCanvas (SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, View view)
		{
			var border = control?.GetBorder ();
			var clipShape = control?.GetClipShape () ?? border;
			if (clipShape != null)
				canvas.ClipPath (clipShape.PathForBounds (dirtyRect).ToSKPath ());
		}

		public static void DrawBackground (SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, View view)
		{
			control?.DrawBackground (canvas, control.GetBackgroundColor (Color.Transparent), dirtyRect);
		}

		public static void DrawBorder (SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, View view)
		{
			var shape = control.GetBorder ();
			if (shape == null)
				return;
			control?.DrawBorder (canvas, shape, dirtyRect);
		}

		public static void DrawText (SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, View view)
		{
			var textHandler = control as ITextHandler;
			if (textHandler == null)
				return;
			if (textHandler.TextBlock == null)
				textHandler.TextBlock = textHandler.CreateTextBlock ();
			control?.DrawText (textHandler.TextBlock, canvas, textHandler.VerticalAlignment);
		}

		public static void DrawOverlay (SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, View view)
		{
			var shape = control.GetOverlay ();
			if (shape == null)
				return;
			control?.DrawOverlay (canvas, shape, dirtyRect);
		}

		public static void Redraw (IViewHandler viewHandler, View virtualView)
		{
			var control = viewHandler as SkiaControl;
			control.Invalidate ();
		}

		public static void MapResetText (IViewHandler viewHandler, View virtualView)
		{
			var textHandler = viewHandler as ITextHandler;
			if (textHandler == null)
				return;
			textHandler.TextBlock = null;
			virtualView.InvalidateMeasurement ();
			Redraw (viewHandler, virtualView);
		}

		public static void MapBackgroundColor (IViewHandler viewHandler, View virtualView)
		{
			var control = viewHandler as SkiaControl;
			control.Background (virtualView.GetBackgroundColor (Color.Transparent));
		}

		public static void MapColorProperty (IViewHandler viewHandler, View virtualView)
		{
			var control = viewHandler as SkiaControl;
			control.Color (virtualView.GetColor (Color.Transparent));
		}

		public static void MapBorderProperty (IViewHandler viewHandler, View virtualView)
		{
			var control = viewHandler as SkiaControl;
			control.Border (virtualView.GetBorder ());
		}

		public static void MapShadowProperty (IViewHandler viewHandler, View virtualView)
		{
			var control = viewHandler as SkiaControl;
			control.SetEnvironment (EnvironmentKeys.View.Shadow, virtualView.GetShadow (), false);
		}

		public static void MapClipShapeProperty (IViewHandler viewHandler, View virtualView)
		{
			var control = viewHandler as SkiaControl;
			control.ClipShape (virtualView.GetClipShape ());
		}
		public static void MapOverlayProperty (IViewHandler viewHandler, View virtualView)
		{
			var control = viewHandler as SkiaControl;
			control.Overlay (virtualView.GetOverlay ());
		}

		protected override void ControlStateChanged()
		{
			if(VirtualView is IControlState control)
			{
				control.CurrentState = CurrentState;
				control.StateChanged?.Invoke(CurrentState);
			}
			base.ControlStateChanged();
		}
	}
}
