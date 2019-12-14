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
		};

		public TextHandler() : base(Mapper)
		{
		}

		protected override WPFLabel CreateView() => new WPFLabel();

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
	}
}
