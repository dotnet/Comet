using AppKit;
using System.Maui.Mac.Extensions;

namespace System.Maui.Mac.Handlers
{
	public class TextHandler : AbstractControlHandler<Text, NSTextField>
	{
		public static readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>(ViewHandler.Mapper)
		{
			[nameof(Text.Value)] = MapValueProperty,
			[nameof(EnvironmentKeys.Text.Alignment)] = MapTextAlignmentProperty,
			[EnvironmentKeys.Fonts.Family] = MapFontProperty,
			[EnvironmentKeys.Fonts.Italic] = MapFontProperty,
			[EnvironmentKeys.Fonts.Size] = MapFontProperty,
			[EnvironmentKeys.Fonts.Weight] = MapFontProperty,
			[EnvironmentKeys.Colors.Color] = MapColorProperty,
		};

		private static FontAttributes DefaultFont;
		private static Color DefaultColor;

		public TextHandler() : base(Mapper)
		{
		}

		protected override NSTextField CreateView()
		{
			var textField = new NSTextField();

			if (DefaultColor == null)
			{
				DefaultFont = textField.Font.ToFont();
				DefaultColor = textField.TextColor.ToColor();
			}

			textField.Editable = false;
			textField.Bezeled = false;
			textField.DrawsBackground = false;
			textField.Selectable = false;

			return textField;
		}

		protected override void DisposeView(NSTextField nativeView)
		{

		}

		public static void MapValueProperty(IViewHandler viewHandler, Text virtualView)
		{
			var nativeView = (NSTextField)viewHandler.NativeView;
			nativeView.StringValue = virtualView.Value?.CurrentValue ?? string.Empty;
			virtualView.InvalidateMeasurement();
		}

		public static void MapTextAlignmentProperty(IViewHandler viewHandler, Text virtualView)
		{
			var nativeView = (NSTextField)viewHandler.NativeView;
			var textAlignment = virtualView.GetTextAlignment();
			nativeView.Alignment = textAlignment.ToNSTextAlignment();
			virtualView.InvalidateMeasurement();
		}

		public static void MapFontProperty(IViewHandler viewHandler, Text virtualView)
		{
			var nativeView = (NSTextField)viewHandler.NativeView;
			var font = virtualView.GetFont(DefaultFont);
			nativeView.Font = font.ToNSFont();
			virtualView.InvalidateMeasurement();
		}

		public static void MapColorProperty(IViewHandler viewHandler, Text virtualView)
		{
			var nativeView = (NSTextField)viewHandler.NativeView;
			var color = virtualView.GetColor(DefaultColor);
			var nativeColor = nativeView.TextColor.ToColor();
			if (!color.Equals(nativeColor))
				nativeView.TextColor = color.ToNSColor();
		}
	}
}
