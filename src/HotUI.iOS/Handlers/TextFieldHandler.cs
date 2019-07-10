using System;
using UIKit;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.iOS
{
    public class TextFieldHandler : AbstractHandler<TextField, UITextField>
    {
        public static readonly PropertyMapper<TextField> Mapper = new PropertyMapper<TextField>(ViewHandler.Mapper)
        {
            [nameof(TextField.Text)] = MapTextProperty
        };
        
        public TextFieldHandler() : base(Mapper)
        {

        }

        protected override UITextField CreateView()
        {
            var textField = new UITextField();
            textField.EditingDidEnd += EntryHandler_EditingDidEnd;

            textField.ShouldReturn = s =>
            {
                textField.ResignFirstResponder();
                return true;
            };
            return textField;
        }
        
        private void EntryHandler_EditingDidEnd(object sender, EventArgs e)
        {
            VirtualView?.Completed(TypedNativeView.Text);
        }
        
        public static bool MapTextProperty(IViewHandler viewHandler, TextField virtualView)
        {
            var nativeView = (UITextField) viewHandler.NativeView;
            nativeView.Text = virtualView.Text;
            nativeView.SizeToFit();
            return true;
        }
    }
}