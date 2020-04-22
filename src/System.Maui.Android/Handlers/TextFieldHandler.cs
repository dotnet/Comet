using System;
using Android.Content;
using Android.Widget;

namespace System.Maui.Android.Handlers
{
	public class TextFieldHandler : AbstractControlHandler<Entry, EditText>
	{
		public static readonly PropertyMapper<Entry> Mapper = new PropertyMapper<Entry>(ViewHandler.Mapper)
		{
			[nameof(Entry.Text)] = MapTextProperty,
			[EnvironmentKeys.Colors.Color] = MapColorProperty,
		};

		public TextFieldHandler() : base(Mapper)
		{
		}
		static Color DefaultColor;
		protected override EditText CreateView(Context context)
		{
			var editText = new EditText(context);
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

		public static void MapTextProperty(IViewHandler viewHandler, Entry virtualView)
		{
			var nativeView = (EditText)viewHandler.NativeView;
			nativeView.Text = virtualView.Text?.CurrentValue ?? string.Empty;
		}

		public static void MapColorProperty(IViewHandler viewHandler, Entry virtualView)
		{
			var textView = (EditText)viewHandler.NativeView;
			var color = virtualView.GetColor(DefaultColor).ToColor();
			textView.SetTextColor(color);
		}

		public static void MapTextAlignmentProperty(IViewHandler viewHandler, Entry virtualView)
		{
			var nativeView = (EditText)viewHandler.NativeView;
			var textAlignment = virtualView.GetTextAlignment();
			nativeView.TextAlignment = textAlignment.ToAndroidTextAlignment();
			virtualView.InvalidateMeasurement();
		}
	}
}
