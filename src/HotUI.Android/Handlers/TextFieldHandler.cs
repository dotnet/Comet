using System;
using Android.Content;
using Android.Widget;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public class TextFieldHandler : AbstractHandler<TextField, EditText>
    {
        public static readonly PropertyMapper<TextField> Mapper = new PropertyMapper<TextField>(ViewHandler.Mapper)
        {
            [nameof(TextField.Text)] = MapTextProperty
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

        private void HandleTextChanged(object sender, EventArgs e) => VirtualView?.Completed(TypedNativeView.Text);

        public static bool MapTextProperty(IViewHandler viewHandler, TextField virtualView)
        {
            var nativeView = (EditText) viewHandler.NativeView;
            nativeView.Text = virtualView.Text;
            return true;
        }
    }
}