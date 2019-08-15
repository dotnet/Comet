using System;
using Android.Content;
using Android.Widget;

namespace Comet.Android.Handlers
{
    public class TextFieldHandler : AbstractControlHandler<TextField, EditText>
    {
        public static readonly PropertyMapper<TextField> Mapper = new PropertyMapper<TextField>(ViewHandler.Mapper)
        {
            [nameof(TextField.Text)] = MapTextProperty,
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

        public static void MapTextProperty(IViewHandler viewHandler, TextField virtualView)
        {
            var nativeView = (EditText) viewHandler.NativeView;
            nativeView.Text = virtualView.Text;
        }

        public static void MapColorProperty(IViewHandler viewHandler, TextField virtualView)
        {
            var textView = viewHandler.NativeView as EditText;
            var color = virtualView.GetColor(DefaultColor).ToColor();
            textView.SetTextColor(color);

        }
    }
}