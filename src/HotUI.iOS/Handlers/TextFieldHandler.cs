using System;
using UIKit;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.iOS.Handlers
{
    public class TextFieldHandler : AbstractControlHandler<TextField, UITextField>
    {
        public static readonly PropertyMapper<TextField> Mapper = new PropertyMapper<TextField>(ViewHandler.Mapper)
        {
            [nameof(TextField.Text)] = MapTextProperty,
            [nameof(SecureField.Placeholder)] = MapPlaceholderProperty
        };
        
        public TextFieldHandler() : base(Mapper)
        {

        }

        protected override UITextField CreateView()
        {
            var textField = new UITextField();
            textField.EditingDidEnd += HandleEditingDidEnd;
            textField.EditingChanged += HandleEditingChanged;
            
            textField.ShouldReturn = s =>
            {
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
            var nativeView = (UITextField) viewHandler.NativeView;
            nativeView.Text = virtualView.Text;
            nativeView.SizeToFit();
        }
        
        public static void MapPlaceholderProperty(IViewHandler viewHandler, TextField virtualView)
        {
            var nativeView = (UITextField) viewHandler.NativeView;
            nativeView.Placeholder = virtualView.Placeholder;
            nativeView.SizeToFit();
        }
    }
}