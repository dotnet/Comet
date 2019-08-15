using Windows.UI.Xaml.Controls;
using UWPTextField = Windows.UI.Xaml.Controls.TextBox;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Comet.UWP.Handlers
{
    public class TextFieldHandler : AbstractControlHandler<TextField, UWPTextField>
    {
        public static readonly PropertyMapper<TextField> Mapper = new PropertyMapper<TextField>()
        {
            [nameof(TextField.Text)] = MapTextProperty
        };

        public TextFieldHandler() : base(Mapper)
        {
        }

        protected override UWPTextField CreateView()
        {
            var textField = new UWPTextField();
            textField.TextChanged += HandleTextChanged;
            return textField;
        }

        private void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            VirtualView?.OnEditingChanged?.Invoke(TypedNativeView.Text);
            VirtualView?.OnCommit?.Invoke(TypedNativeView.Text);
        }

        protected override void DisposeView(UWPTextField textField)
        {
            textField.TextChanged -= HandleTextChanged;
        }

        public static void MapTextProperty(IViewHandler viewHandler, TextField virtualView)
        {
            var nativeView = (UWPTextField)viewHandler.NativeView;
            nativeView.Text = virtualView.Text ?? "";
        }
    }
}
