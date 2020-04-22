using WPFLabel = System.Windows.Controls.Label;
// ReSharper disable ClassNeverInstantiated.Global

namespace System.Maui.WPF.Handlers
{
	public class TextHandler : AbstractControlHandler<Label, WPFLabel>
	{
		public static readonly PropertyMapper<Label> Mapper = new PropertyMapper<Label>()
		{
			[nameof(Label.Value)] = MapValueProperty,
			[nameof(EnvironmentKeys.Text.Alignment)] = MapTextAlignmentProperty,
		};

		public TextHandler() : base(Mapper)
		{
		}

		protected override WPFLabel CreateView() => new WPFLabel();

		protected override void DisposeView(WPFLabel nativeView)
		{
		}

		public static void MapValueProperty(IViewHandler viewHandler, Label virtualView)
		{
			var nativeView = (WPFLabel)viewHandler.NativeView;
			nativeView.Content = virtualView.Value?.CurrentValue ?? string.Empty;
			virtualView.InvalidateMeasurement();
		}

		public static void MapTextAlignmentProperty(IViewHandler viewHandler, Label virtualView)
		{
			var nativeView = (WPFLabel)viewHandler.NativeView;
			var textAlignment = virtualView.GetTextAlignment();
			nativeView.HorizontalContentAlignment = textAlignment.ToHorizontalAlignment();
			virtualView.InvalidateMeasurement();
		}
	}
}
