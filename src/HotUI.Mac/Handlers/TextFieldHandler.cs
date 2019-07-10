using System;
using AppKit;

namespace HotUI.Mac.Handlers
{
    public class TextFieldHandler : AbstractHandler<TextField,NSTextField>
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
            textField.EditingEnded += EntryHandler_Ended;
            return textField;
        }

        void EntryHandler_Ended(object sender, EventArgs e) => VirtualView?.Completed(TypedNativeView.StringValue);

        public static void MapTextProperty(IViewHandler viewHandler, TextField virtualView)
        {
            var nativeView = (NSTextField) viewHandler.NativeView;
            nativeView.StringValue = virtualView.Text;
            nativeView.SizeToFit();
        }
    }
}