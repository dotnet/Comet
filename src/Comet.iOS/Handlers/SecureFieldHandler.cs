using System;
using UIKit;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Comet.iOS.Handlers
{
	public class SecureFieldHandler : AbstractControlHandler<SecureField, UITextField>
	{
		public static readonly PropertyMapper<SecureField> Mapper = new PropertyMapper<SecureField>(ViewHandler.Mapper)
		{
			[nameof(SecureField.Text)] = MapTextProperty,
			[nameof(SecureField.Placeholder)] = MapPlaceholderProperty
		};

		public SecureFieldHandler() : base(Mapper)
		{

		}

		protected override UITextField CreateView()
		{
			var textField = new UITextField();
			textField.SecureTextEntry = true;
			textField.EditingDidEnd += EntryHandler_EditingDidEnd;

			textField.ShouldReturn = s => {
				textField.ResignFirstResponder();
				return true;
			};

			return textField;
		}

		protected override void DisposeView(UITextField nativeView)
		{
			nativeView.EditingDidEnd -= EntryHandler_EditingDidEnd;
			nativeView.ShouldReturn = null;
		}

		private void EntryHandler_EditingDidEnd(object sender, EventArgs e)
		{
			VirtualView?.OnCommit?.Invoke(TypedNativeView.Text);
		}

		public static void MapTextProperty(IViewHandler viewHandler, SecureField virtualView)
		{
			var nativeView = (UITextField)viewHandler.NativeView;
			nativeView.Text = virtualView.Text?.CurrentValue ?? string.Empty;
			virtualView.InvalidateMeasurement();
		}

		public static void MapPlaceholderProperty(IViewHandler viewHandler, SecureField virtualView)
		{
			var nativeView = (UITextField)viewHandler.NativeView;
			nativeView.Placeholder = virtualView.Placeholder?.CurrentValue;
			virtualView.InvalidateMeasurement();
		}
	}
}
