using System;
using AppKit;

namespace HotUI.Mac.Handlers
{
    public class TextFieldHandler : AbstractHandler<TextField, NSTextField>
    {
        public static readonly PropertyMapper<TextField> Mapper = new PropertyMapper<TextField>(ViewHandler.Mapper)
        {
            [nameof(TextField.Text)] = MapTextProperty
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

        private void HandleEditingChanged(object sender, EventArgs e)
        {
            VirtualView?.OnEditingChanged?.Invoke(TypedNativeView.StringValue);
        }

        void HandleEditingEnded(object sender, EventArgs e)
        {
            VirtualView?.OnCommit?.Invoke(TypedNativeView.StringValue);
        }

        public static void MapTextProperty(IViewHandler viewHandler, TextField virtualView)
        {
            var nativeView = (NSTextField) viewHandler.NativeView;
            nativeView.StringValue = virtualView.Text;
            nativeView.SizeToFit();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            if (TypedNativeView != null)
            {
                TypedNativeView.EditingEnded -= HandleEditingEnded;
                TypedNativeView.Changed -= HandleEditingChanged;
            }
            base.Dispose(disposing);
        }
    }
}