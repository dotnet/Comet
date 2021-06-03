using System;
using WPFLabel = System.Windows.Controls.Label;
// ReSharper disable ClassNeverInstantiated.Global

namespace Comet.WPF.Handlers
{
	public class TextHandler : AbstractControlHandler<Text, WPFLabel>
	{
		public static readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>()
		{
			[nameof(Text.Value)] = MapValueProperty,
			[nameof(EnvironmentKeys.Text.Alignment)] = MapTextAlignmentProperty,
			[nameof(EnvironmentKeys.View.StyleId)] = MapTextStyleProperty,
			[nameof(EnvironmentKeys.Fonts.Weight)] = MapTextWeightProperty
		};

		public static FontAttributes DefaultFont;

		public TextHandler() : base(Mapper)
		{
		}

		protected override WPFLabel CreateView()
		{
			DefaultFont = new FontAttributes()
			{
				Family = "Segoe UI",
				Size = 12,
				Weight = Weight.Regular,
				Italic = false
			};

			return new WPFLabel();
		}

		protected override void DisposeView(WPFLabel nativeView)
		{
		}

		public static void MapValueProperty(IViewHandler viewHandler, Text virtualView)
		{
			var nativeView = (WPFLabel)viewHandler.NativeView;
			nativeView.Content = virtualView.Value?.CurrentValue ?? string.Empty;
			virtualView.InvalidateMeasurement();
		}

		public static void MapTextAlignmentProperty(IViewHandler viewHandler, Text virtualView)
		{
			var nativeView = (WPFLabel)viewHandler.NativeView;
			var textAlignment = virtualView.GetTextAlignment();
			nativeView.HorizontalContentAlignment = textAlignment.ToHorizontalAlignment();
			virtualView.InvalidateMeasurement();
		}

		public static void MapTextStyleProperty(IViewHandler viewHandler, Text virtualText)
		{
			var nativeView = (WPFLabel)viewHandler.NativeView;
			var fontSize = virtualText.GetWpfFontSize();
			nativeView.FontSize = fontSize;
		}

		public static void MapTextWeightProperty(IViewHandler viewHandler, Text virtualText)
		{
			var nativeView = (WPFLabel)viewHandler.NativeView;
			var font = virtualText.GetFont(DefaultFont);
			nativeView.FontFamily = new System.Windows.Media.FontFamily(font.Family);
			nativeView.FontSize = font.Size;
			nativeView.FontWeight = font.Weight.GetWpfFontWeight();
			nativeView.FontStyle = font.Italic ? System.Windows.FontStyles.Italic : System.Windows.FontStyles.Normal;
		}
	}
}
