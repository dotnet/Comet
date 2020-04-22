using System.Windows.Controls;
using WPFTextField = System.Windows.Controls.TextBox;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace System.Maui.WPF.Handlers
{
	public class TextFieldHandler : AbstractControlHandler<Entry, WPFTextField>
	{
		public static readonly PropertyMapper<Entry> Mapper = new PropertyMapper<Entry>()
		{
			[nameof(Entry.Text)] = MapTextProperty,
			[nameof(EnvironmentKeys.Text.Alignment)] = MapTextAlignmentProperty,
		};

		public TextFieldHandler() : base(Mapper)
		{
		}

		protected override WPFTextField CreateView()
		{
			var textField = new WPFTextField();
			textField.TextChanged += HandleTextChanged;
			return textField;
		}

		protected override void DisposeView(WPFTextField textField)
		{
			textField.TextChanged -= HandleTextChanged;
		}

		private void HandleTextChanged(object sender, TextChangedEventArgs e)
		{
			VirtualView?.OnEditingChanged?.Invoke(TypedNativeView.Text);
			VirtualView?.OnCommit?.Invoke(TypedNativeView.Text);
		}

		public static void MapTextProperty(IViewHandler viewHandler, Entry virtualView)
		{
			var nativeView = (WPFTextField)viewHandler.NativeView;
			nativeView.Text = virtualView.Text?.CurrentValue ?? string.Empty;
			virtualView.InvalidateMeasurement();
		}
		public static void MapTextAlignmentProperty(IViewHandler viewHandler, Entry virtualView)
		{
			var nativeView = (WPFTextField)viewHandler.NativeView;
			var textAlignment = virtualView.GetTextAlignment();
			nativeView.HorizontalContentAlignment = textAlignment.ToHorizontalAlignment();
			virtualView.InvalidateMeasurement();
		}
	}
}
