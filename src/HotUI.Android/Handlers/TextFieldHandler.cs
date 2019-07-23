using System;
using Android.Content;
using Android.Widget;

namespace HotUI.Android.Handlers
{
    public class TextFieldHandler : AbstractControlHandler<TextField, EditText>
    {
        public static readonly PropertyMapper<TextField> Mapper = new PropertyMapper<TextField>(ViewHandler.Mapper)
        {
            [nameof(TextField.TextValue)] = MapTextProperty
        };

        public TextFieldHandler() : base(Mapper)
        {
        }

        protected override EditText CreateView(Context context)
        {
            var editText = new EditText(context);
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
            nativeView.Text = virtualView.TextValue;
        }
    }
}