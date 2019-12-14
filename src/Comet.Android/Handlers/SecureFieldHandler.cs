using System;
using Android.Content;
using Android.Text;
using Android.Widget;

namespace Comet.Android.Handlers
{
	public class SecureFieldHandler : AbstractControlHandler<SecureField, EditText>
	{
		public static readonly PropertyMapper<SecureField> Mapper = new PropertyMapper<SecureField>(ViewHandler.Mapper)
		{
			[nameof(TextField.Text)] = MapTextProperty,
			[EnvironmentKeys.Colors.Color] = MapColorProperty,
		};

		public SecureFieldHandler() : base(Mapper)
		{
		}
		static Color DefaultColor;
		protected override EditText CreateView(Context context)
		{
			var editText = new EditText(context);
			editText.InputType = InputTypes.TextVariationPassword;
			if (DefaultColor == null)
			{
				DefaultColor = editText.CurrentTextColor.ToColor();
			}
			editText.TextChanged += HandleTextChanged;
			return editText;
		}

		protected override void DisposeView(EditText nativeView)
		{
			nativeView.TextChanged -= HandleTextChanged;
		}

		private void HandleTextChanged(object sender, EventArgs e)
		{
			VirtualView?.OnCommit?.Invoke(TypedNativeView.Text);
		}

		public static void MapTextProperty(IViewHandler viewHandler, SecureField virtualView)
		{
			var nativeView = (EditText)viewHandler.NativeView;
			nativeView.Text = virtualView.Text?.CurrentValue ?? string.Empty;
		}

		public static void MapColorProperty(IViewHandler viewHandler, SecureField virtualView)
		{
			var textView = (EditText)viewHandler.NativeView;
			var color = virtualView.GetColor(DefaultColor).ToColor();
			textView.SetTextColor(color);
		}

		public static void MapTextAlignmentProperty(IViewHandler viewHandler, SecureField virtualView)
		{
			var nativeView = (EditText)viewHandler.NativeView;
			var textAlignment = virtualView.GetTextAlignment();
			nativeView.TextAlignment = textAlignment.ToAndroidTextAlignment();
			virtualView.InvalidateMeasurement();
		}
	}
}
