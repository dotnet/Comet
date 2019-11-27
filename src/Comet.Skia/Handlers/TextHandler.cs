using System;
using System.Drawing;
using SkiaSharp;

namespace Comet.Skia
{
	public class TextHandler : SkiaControl
	{
		public static readonly PropertyMapper<SkiaView> Mapper = new PropertyMapper<SkiaView>()
		{
			[nameof(Comet.Text.Value)] = MapValueProperty,
			[nameof(EnvironmentKeys.Text.Alignment)] = MapValueProperty,
			[EnvironmentKeys.Fonts.Family] = MapValueProperty,
			[EnvironmentKeys.Fonts.Italic] = MapValueProperty,
			[EnvironmentKeys.Fonts.Size] = MapValueProperty,
			[EnvironmentKeys.Fonts.Weight] = MapValueProperty,
			[EnvironmentKeys.Colors.Color] = MapValueProperty,
			[EnvironmentKeys.LineBreakMode.Mode] = MapValueProperty,
		};
		public TextHandler() : base(Mapper)
		{

		}
		static float hPadding = 40;
		static float minHPadding = 10;
		static float vPadding = 10;

		static FontAttributes defaultFont = new FontAttributes
		{
			Family = "System",
			Size = 16,
			Weight = Weight.Regular,
		};

		Text VirtualText => (Text)VirtualView;
		public override void Draw(SKCanvas canvas, RectangleF dirtyRect)
		{
			base.Draw(canvas, dirtyRect);

			DrawText(VirtualText.Value, canvas, VirtualView.GetFont(defaultFont), VirtualView.GetColor(Color.Black),
				VirtualView.GetTextAlignment(TextAlignment.Center) ?? TextAlignment.Center,
				VirtualView.GetLineBreakMode(LineBreakMode.NoWrap), VerticalAlignment.Center);
		}

		public override SizeF Measure(SizeF availableSize)
		{
			var text = VirtualText;
			var size = SkiaTextHelper.GetTextSize(text.Value, text.GetFont(defaultFont),
				text.GetTextAlignment(TextAlignment.Center) ?? TextAlignment.Center,
				text.GetLineBreakMode(LineBreakMode.NoWrap), availableSize.Width - minHPadding);
			var margin = text.GetMargin();
			if (size.Width > 300)
				Console.WriteLine("Hi");
			return new SizeF(size.Width + hPadding, size.Height + (vPadding * 2));
		}

		public override string AccessibilityText() => VirtualText?.Value;

		public static void MapValueProperty(IViewHandler viewHandler, SkiaView virtualView)
		{
			var control = virtualView as SkiaControl;
			control.VirtualView.InvalidateMeasurement();
			virtualView.Invalidate();
		}
	}
}
