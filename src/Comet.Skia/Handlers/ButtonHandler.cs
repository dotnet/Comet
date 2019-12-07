using System;
using SkiaSharp;
using System.Linq;
using System.Drawing;
using Topten.RichTextKit;
using Comet.Skia.Internal;
using Comet.Internal;

namespace Comet.Skia
{
	public class ButtonHandler : SKiaAbstractControlHandler<Button>, ITextHandler
	{
		public static readonly DrawMapper<Button> DrawingMapper = new DrawMapper<Button>(SkiaControl.DrawMapper)
		{
			[SkiaEnvironmentKeys.Background] = DrawAccentLayer,
		};

		public static new readonly PropertyMapper<Button> Mapper = new PropertyMapper<Button>(SkiaControl.Mapper)
		{
			[nameof(Button.Text)] = MapResetText,
			[EnvironmentKeys.Colors.Color] = MapResetText
		};

		static float hPadding = 40;
		static float minHPadding = 10;
		static float vPadding = 10;

		static FontAttributes defaultFont = new FontAttributes
		{
			Family = "System",
			Size = 14,
			Weight = Weight.Bold,
		};
		
		public ButtonHandler() : base(DrawingMapper, Mapper)
        {

        }


        public override SizeF Measure(SizeF availableSize)
		{
			TextBlock.MaxHeight = null;
			TextBlock.MaxWidth = availableSize.Width - minHPadding;
			TextBlock.Layout();
			return new SizeF(TextBlock.MeasuredWidth + hPadding, TextBlock.MeasuredHeight + vPadding);
		}

		public override void LayoutSubviews(RectangleF frame)
		{
			TextBlock.MaxHeight = frame.Height;
			TextBlock.MaxWidth = frame.Width;
			TextBlock.Layout();
			base.LayoutSubviews(frame);
		}

		const string accentRadius = "Button.AccentRadius";
		protected void DrawAccentLayer(SKCanvas canvas, RectangleF dirtyRect)
        {
			var radius = this.GetEnvironment<float>(accentRadius);
			if (radius <= 0)
			{
				DrawBackground(canvas, this.GetBackgroundColor());
				return;
			}

			var point = (CurrentTouchPoint == PointF.Empty ? dirtyRect.Center() : CurrentTouchPoint).ToSKPoint();
			var defaultColor = TypedVirtualView.GetBackgroundColor(Color.Transparent, state: ControlState.Default);
			var backgroundColor = TypedVirtualView.GetBackgroundColor(state: CurrentState)
				?? defaultColor.Lerp(Color.Grey, .5);
			DrawBackground(canvas, defaultColor);
			var paint = new SKPaint();
			paint.Color =  backgroundColor.ToSKColor();
			var circleRadius = 5f.Lerp(Math.Max(dirtyRect.Width, dirtyRect.Height) * 2f, radius);
			canvas.DrawCircle(point, circleRadius, paint);
        }


		public override void EndInteraction(PointF[] points, bool contained)
		{
			if (contained)
				TypedVirtualView?.OnClick?.Invoke();
			base.EndInteraction(points, contained);
		}

        protected override void ControlStateChanged()
		{
			var endBackground = TypedVirtualView.GetBackgroundColor(state: CurrentState)
				//If null, get the normal state and lerp that puppy
				?? TypedVirtualView.GetBackgroundColor(Color.Transparent, state: ControlState.Default).Lerp(Color.Grey, .1).WithAlpha(.5f);

			var endPadding = (CurrentState == ControlState.Pressed) ? new Thickness(.5f) : new Thickness();
			float radius = (CurrentState == ControlState.Pressed) ? 1 : 0;
			this.Animate(x => {
               // x.Color(end);
                //x.Background(endBackground);
				x.Padding(endPadding);
				x.SetEnvironment(accentRadius, radius);
            });

        }

		TextBlock textBlock;
		public TextBlock TextBlock
		{
			get => textBlock ??= CreateTextBlock();
			set => textBlock = value;
		}

		public VerticalAlignment VerticalAlignment => VerticalAlignment.Center;

		public override string AccessibilityText() => TypedVirtualView?.Text;

		public TextBlock CreateTextBlock()
		{
			var font = VirtualView.GetFont(defaultFont);
			var color = VirtualView.GetColor(Color.Black);
			var alignment = VirtualView.GetTextAlignment(TextAlignment.Center) ?? TextAlignment.Center;
			var tb = new TextBlock();
			tb.AddText(TypedVirtualView.Text, font.ToStyle(color));
			tb.MaxWidth = VirtualView.Frame.Width;
			tb.MaxHeight = VirtualView.Frame.Height;
			tb.MaxLines = null;
			tb.Alignment = alignment.ToTextAlignment();
			tb.Layout();
			return tb;
		}

		public static void DrawAccentLayer(SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, Button view)
        {
			var button = control as ButtonHandler;
			if (button == null)
				return;
			button.DrawAccentLayer(canvas, dirtyRect);
        }


	}
}
