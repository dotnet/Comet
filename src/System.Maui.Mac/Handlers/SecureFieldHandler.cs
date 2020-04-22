using System;
using AppKit;

namespace System.Maui.Mac.Handlers
{
	public class SecureFieldHandler : AbstractControlHandler<Entry, NSSecureTextField>
	{
		public static readonly PropertyMapper<Entry> Mapper = new PropertyMapper<Entry>(ViewHandler.Mapper)
		{
			[nameof(Entry.Text)] = MapTextProperty
		};

		public SecureFieldHandler() : base(Mapper)
		{
		}

		protected override NSSecureTextField CreateView()
		{
			var textField = new NSSecureTextField();
			textField.EditingEnded += HandleEditingEnded;
			textField.Changed += HandleEditingChanged;
			return textField;
		}

		protected override void DisposeView(NSSecureTextField nativeView)
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
			var nativeView = (NSSecureTextField)viewHandler.NativeView;
			nativeView.StringValue = virtualView.Text;
			virtualView.InvalidateMeasurement();
		}
	}
}
