using Windows.UI.Xaml.Media;
using UWPLabel = Windows.UI.Xaml.Controls.TextBlock;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Comet.UWP.Handlers
{
	public class TextHandler : AbstractControlHandler<Text, UWPLabel>
	{
		public static readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>()
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
		static Color DefaultColor;

		public TextHandler() : base(Mapper)
		{
		}

		protected override UWPLabel CreateView()
		{
			var textBlock = new UWPLabel();

			if (DefaultColor == null)
			{
				var brush = textBlock.Foreground;
				if (brush is SolidColorBrush)
				{
					DefaultColor = ((SolidColorBrush)brush).Color.ToColor();
				}
			}
			if (DefaultFont == null)
			{
				DefaultFont = textBlock.ToFont();
			}

			return textBlock;
		}

		protected override void DisposeView(UWPLabel nativeView)
		{

		}

		public static void MapFontProperty(IViewHandler viewHandler, Text virtualView)
		{
			var nativeView = (UWPLabel)viewHandler.NativeView;
			var font = virtualView.GetFont(DefaultFont);
			nativeView.SetFont(font);
			virtualView.InvalidateMeasurement();
		}

		public static void MapColorProperty(IViewHandler viewHandler, Text virtualView)
		{
			var nativeView = (UWPLabel)viewHandler.NativeView;
			var color = virtualView.GetColor(DefaultColor);
			nativeView.SetFontColor(color);
		}

		public static void MapValueProperty(IViewHandler viewHandler, Text virtualView)
		{
			var nativeView = (UWPLabel)viewHandler.NativeView;
			nativeView.Text = virtualView.Value.CurrentValue;
			virtualView.InvalidateMeasurement();
		}

		public static void MapTextAlignmentProperty(IViewHandler viewHandler, Text virtualView)
		{
			var nativeView = (UWPLabel)viewHandler.NativeView;
			var textAlignment = virtualView.GetTextAlignment();
			nativeView.HorizontalTextAlignment = textAlignment.ToTextAlignment();
			virtualView.InvalidateMeasurement();
		}
	}
}
