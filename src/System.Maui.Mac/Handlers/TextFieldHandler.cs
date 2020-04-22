using System;
using AppKit;
using System.Maui.Mac.Extensions;

namespace System.Maui.Mac.Handlers
{
	public class TextFieldHandler : AbstractControlHandler<Entry, NSTextField>
	{
		public static readonly PropertyMapper<Entry> Mapper = new PropertyMapper<Entry>(ViewHandler.Mapper)
		{
			[nameof(Entry.Text)] = MapTextProperty,
			[nameof(EnvironmentKeys.Text.Alignment)] = MapTextAlignmentProperty,
		};

		public TextFieldHandler() : base(Mapper)
		{
		}

		protected override NSTextField CreateView()
		{
			var textField = new NSTextField();
			textField.EditingEnded += HandleEditingEnded;
			textField.Changed += HandleEditingChanged;
			return textField;
		}

		protected override void DisposeView(NSTextField nativeView)
		{
			nativeView.EditingEnded -= HandleEditingEnded;
			nativeView.Changed -= HandleEditingChanged;
		}

		private void HandleEditingChanged(object sender, EventArgs e)
		{
			VirtualView?.OnEditingChanged?.Invoke(TypedNativeView.StringValue);
		}

		void HandleEditingEnded(object sender, EventArgs e)
		{
			VirtualView?.OnCommit?.Invoke(TypedNativeView.StringValue);
		}

		public static void MapTextProperty(IViewHandler viewHandler, Entry virtualView)
		{
			var nativeView = (NSTextField)viewHandler.NativeView;
			nativeView.StringValue = virtualView.Text?.CurrentValue ?? string.Empty;
			virtualView.InvalidateMeasurement();
		}

		public static void MapTextAlignmentProperty(IViewHandler viewHandler, Entry virtualView)
		{
			var nativeView = (NSTextField)viewHandler.NativeView;
			var textAlignment = virtualView.GetTextAlignment();
			nativeView.Alignment = textAlignment.ToNSTextAlignment();
			virtualView.InvalidateMeasurement();
		}
	}
}
