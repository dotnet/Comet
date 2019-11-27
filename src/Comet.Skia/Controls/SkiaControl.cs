using System;
using System.Drawing;
using Comet.Skia.Internal;
using SkiaSharp;
using TextBlock = Topten.RichTextKit.TextBlock;

namespace Comet.Skia
{
	public abstract class SkiaControl : SkiaView
	{

		protected SkiaControl() : this(new PropertyMapper<SkiaView>()) { }

		protected SkiaControl(PropertyMapper<SkiaView> mapper) : base(mapper) { }

		public View VirtualView { get; private set; }

		public override SizeF Measure(SizeF availableSize) => new SizeF(100, 44);

		public virtual void SetView(View view)
		{
			VirtualView = view;
			this.Parent = view?.Parent;
		}

		public override void Draw(SKCanvas canvas, RectangleF dirtyRect)
		{
			canvas.Clear(Color.Transparent.ToSKColor());
			if (VirtualView == null)
				return;
			canvas.Save();
			var border = VirtualView?.GetBorder();
			var backgroundColor = VirtualView?.GetBackgroundColor();
			if (border != null)
				DrawBorder(canvas, border, dirtyRect, backgroundColor);
			else
				DrawBackground(canvas, backgroundColor);
			canvas.Restore();
		}

		protected virtual void DrawBorder(SKCanvas canvas, Shape shape, RectangleF rect, Color backgroundColor)
		{
			var strokeColor = shape.GetStrokeColor(VirtualView, Color.Black);
			var strokeWidth = shape.GetLineWidth(VirtualView, 1);
			canvas.DrawShape(shape, rect, strokeColor: strokeColor, strokeWidth: strokeWidth, fill: backgroundColor ?? Color.Fuchsia);
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

		public abstract string AccessibilityText();
	}
}
