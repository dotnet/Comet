using System;
using SkiaSharp;
using CButton = Comet.Button;
using System.Linq;
using System.Drawing;
using Topten.RichTextKit;
using Comet.Skia.Internal;

namespace Comet.Skia
{
	public enum ButtonState
	{
		Normal,
		Hover,
		Pressed
	}

	public class ButtonHandler : SKiaAbstractControlHandler<CButton>, ITextHandler
	{
		public static new readonly PropertyMapper<CButton> Mapper = new PropertyMapper<CButton>(SkiaControl.Mapper)
		{
			[nameof(CButton.Text)] = MapResetText,
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

		public ButtonHandler() : base(null, Mapper)
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

		public override bool StartInteraction(PointF[] points)
		{

			ButtonState = ButtonState.Pressed;
			return base.StartInteraction(points);
		}

		public override void HoverInteraction(PointF[] points)
		{
			ButtonState = ButtonState.Hover;
			base.HoverInteraction(points);
		}


		public override void EndInteraction(PointF[] points, bool contained)
		{
			ButtonState = ButtonState.Normal;
			if (contained)
				TypedVirtualView?.OnClick?.Invoke();
			base.EndInteraction(points, contained);
		}

		public override void CancelInteraction()
		{
			ButtonState = ButtonState.Normal;
			base.CancelInteraction();
		}

		ButtonState buttonState = ButtonState.Normal;

		public ButtonState ButtonState
		{
			get => buttonState;
			set
			{
				if (buttonState == value)
					return;
				buttonState = value;
				Invalidate();
			}
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


		public static void MapResetText(IViewHandler viewHandler, CButton virtualView)
		{
			var textHandler = viewHandler as ITextHandler;
			textHandler.TextBlock = null;

			////nativeView.SetTitle(virtualView.Text?.CurrentValue, UIControlState.Normal);
			virtualView.InvalidateMeasurement();
		}
	}
}
