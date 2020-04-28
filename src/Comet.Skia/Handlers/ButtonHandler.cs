using System;
using SkiaSharp;
using System.Linq;
using System.Drawing;
using Topten.RichTextKit;
using Comet.Skia.Internal;
using Comet.Internal;

namespace Comet.Skia
{
	public class ButtonHandler : SkiaAbstractControlHandler<Button>, ITextHandler
	{

		public static new readonly PropertyMapper<Button> Mapper = new PropertyMapper<Button>(SkiaControl.Mapper)
		{
			[nameof(Button.Text)] = MapResetText,
			[EnvironmentKeys.Colors.Color] = MapResetText
		};

		static float hPadding = 40;
		static float minHPadding = 10;
		static float vPadding = 10;

		static readonly FontAttributes defaultFont = new FontAttributes
		{
			Family = SkiaTextHelper.GetDefaultFontFamily,
			Size = 14,
			Weight = Weight.Bold,
		};

		public ButtonHandler() : base(null, Mapper)
		{

		}
		
		public override SizeF GetIntrinsicSize(SizeF availableSize)
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
		SKPoint animationPoint;
		protected override void DrawBackground(SKCanvas canvas, Color defaultBackgroundColor, RectangleF dirtyRect)
		{


			var defaultColor = TypedVirtualView.GetBackgroundColor(Color.Transparent, state: ControlState.Default);

			Color calculateDefaultBackgroundColor()
			{
				var c = defaultColor;
				c = CurrentState switch
				{
					ControlState.Disabled => c.Lerp(Color.Grey, .5),
					ControlState.Hovered => c.Lerp(Color.Grey, .1),
					ControlState.Pressed => c.Lerp(Color.Grey, .5),
					_ => c
				};
				return c;

			};

			var backgroundColor = TypedVirtualView.GetBackgroundColor(state: CurrentState) ?? calculateDefaultBackgroundColor();
			var radius = (this).GetEnvironment<float>(accentRadius);
			if (radius <= 0 || radius >= 1)
			{
				base.DrawBackground(canvas, defaultColor, dirtyRect);
				base.DrawBackground(canvas, backgroundColor, dirtyRect);
				return;
			}
			base.DrawBackground(canvas, defaultColor, dirtyRect);
			var paint = new SKPaint();
			paint.Color = backgroundColor.ToSKColor();
			var circleRadius = 5f.Lerp(Math.Max(dirtyRect.Width, dirtyRect.Height) * 2f, radius);
			canvas.DrawCircle(animationPoint, circleRadius, paint);
		}


		public override void EndInteraction(PointF[] points, bool contained)
		{
			if (contained)
				TypedVirtualView?.OnClick?.Invoke();
			base.EndInteraction(points, contained);
		}

		protected override void ControlStateChanged()
		{
			base.ControlStateChanged();
			animationPoint = (CurrentTouchPoint == PointF.Empty ? this.Frame.Center() : CurrentTouchPoint).ToSKPoint();
			var endPadding = (CurrentState == ControlState.Pressed) ? new Thickness(.5f) : new Thickness();

			//TODO: Make all color stuff state aware
			//var endColor = TypedVirtualView.GetColor(state: CurrentState);

			float radius = (CurrentState == ControlState.Pressed || CurrentState == ControlState.Hovered) ? 1 : 0;
			(this).Animate(x => {
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


	}
}
