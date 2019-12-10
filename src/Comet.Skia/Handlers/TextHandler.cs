using System;
using System.Drawing;
using Comet;
using Comet.Skia.Internal;
using SkiaSharp;
using Topten.RichTextKit;

namespace Comet.Skia
{
	public class TextHandler : SKiaAbstractControlHandler<Text>, ITextHandler
	{
		public static new readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>(SkiaControl.Mapper)
		{
			[nameof(Comet.Text.Value)] = MapResetText,
			[EnvironmentKeys.Colors.Color] = MapResetText
		};

		public TextHandler() : base(null, Mapper)
		{
			TouchEnabled = false;
		}
		static float hPadding = 40;
		static float minHPadding = 10;
		static float vPadding = 10;

		static FontAttributes defaultFont = new FontAttributes
		{
			Family = SkiaTextHelper.GetDefaultFontFamily,
			Size = 16,
			Weight = Weight.Regular,
		};

		public override SizeF Measure(SizeF availableSize)
		{
			TextBlock.MaxHeight = null;
			TextBlock.MaxWidth = availableSize.Width - minHPadding;
			TextBlock.Layout();
			return new SizeF(TextBlock.MeasuredWidth + hPadding, TextBlock.MeasuredHeight + vPadding);
		}

		public override string AccessibilityText() => TypedVirtualView?.Value;


		TextBlock textBlock;
		public TextBlock TextBlock
		{
			get => textBlock ??= CreateTextBlock();
			set => textBlock = value;
		}

		public VerticalAlignment VerticalAlignment => VerticalAlignment.Center;


		public TextBlock CreateTextBlock()
		{
			var font = VirtualView.GetFont(defaultFont);
			var color = VirtualView.GetColor(Color.Black);
			var alignment = VirtualView.GetTextAlignment(TextAlignment.Center) ?? TextAlignment.Center;
			var tb = new TextBlock();
			tb.AddText(TypedVirtualView.Value, font.ToStyle(color));
			tb.MaxWidth = VirtualView.Frame.Width;
			tb.MaxHeight = VirtualView.Frame.Height;
			tb.MaxLines = null;
			tb.Alignment = alignment.ToTextAlignment();
			tb.Layout();
			return tb;
		}

	}
}
