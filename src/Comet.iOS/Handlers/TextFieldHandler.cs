using System;
using UIKit;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Comet.iOS.Handlers
{
	public class TextFieldHandler : AbstractControlHandler<TextField, UITextField>
	{
		public static readonly PropertyMapper<TextField> Mapper = new PropertyMapper<TextField>(ViewHandler.Mapper)
		{
			[nameof(TextField.Text)] = MapTextProperty,
			[nameof(EnvironmentKeys.Text.Alignment)] = MapTextAlignmentProperty,
			[nameof(TextField.Placeholder)] = MapPlaceholderProperty,
			[EnvironmentKeys.Colors.Color] = MapColorProperty,
		};


		private static FontAttributes DefaultFont;
		private static Color DefaultColor;
		public TextFieldHandler() : base(Mapper)
		{

		}

		protected override UITextField CreateView()
		{
			var textField = new UITextField();
			if (DefaultColor == null)
			{
				DefaultFont = textField.Font.ToFont();
				DefaultColor = textField.TextColor.ToColor();
			}

			textField.EditingDidEnd += HandleEditingDidEnd;
			textField.EditingChanged += HandleEditingChanged;

			textField.ShouldReturn = s => {
				textField.ResignFirstResponder();
				return true;
			};

			return textField;
		}

		protected override void DisposeView(UITextField textField)
		{
			textField.EditingDidEnd -= HandleEditingDidEnd;
			textField.EditingChanged -= HandleEditingChanged;
			textField.ShouldReturn = null;
		}

		private void HandleEditingChanged(object sender, EventArgs e)
		{
			VirtualView?.OnEditingChanged?.Invoke(TypedNativeView.Text);
		}

		private void HandleEditingDidEnd(object sender, EventArgs e)
		{
			VirtualView?.OnCommit?.Invoke(TypedNativeView.Text);
		}

		public static void MapTextProperty(IViewHandler viewHandler, TextField virtualView)
		{
			var nativeView = (UITextField)viewHandler.NativeView;
			nativeView.Text = virtualView.Text?.CurrentValue ?? string.Empty;
			virtualView.InvalidateMeasurement();
		}

		public static void MapTextAlignmentProperty(IViewHandler viewHandler, TextField virtualView)
		{
			var nativeView = (UITextField)viewHandler.NativeView;
			var textAlignment = virtualView.GetTextAlignment();
			nativeView.TextAlignment = textAlignment.ToUITextAlignment();
			virtualView.InvalidateMeasurement();
		}

		public static void MapPlaceholderProperty(IViewHandler viewHandler, TextField virtualView)
		{
			var nativeView = (UITextField)viewHandler.NativeView;
			nativeView.Placeholder = virtualView.Placeholder.CurrentValue;
			virtualView.InvalidateMeasurement();
		}
		public static void MapColorProperty(IViewHandler viewHandler, TextField virtualView)
		{
			var nativeView = (UITextField)viewHandler.NativeView;
			var color = virtualView.GetColor(DefaultColor);
			nativeView.TextColor = color.ToUIColor();
		}
	}
}
