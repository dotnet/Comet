using System.Windows.Controls;
using WPFTextField = System.Windows.Controls.TextBox;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Comet.WPF.Handlers
{
    public class TextFieldHandler : AbstractControlHandler<TextField, WPFTextField>
    {
        public static readonly PropertyMapper<TextField> Mapper = new PropertyMapper<TextField>()
        {
            [nameof(TextField.Text)] = MapTextProperty
        };
        
        public TextFieldHandler() : base(Mapper)
        {
        }

        protected override WPFTextField CreateView()
        {
            var textField = new WPFTextField();
            textField.TextChanged += HandleTextChanged;
            return textField;
        }

        protected override void DisposeView(WPFTextField textField)
        {
            textField.TextChanged -= HandleTextChanged;
        }

        private void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            VirtualView?.OnEditingChanged?.Invoke(TypedNativeView.Text);
            VirtualView?.OnCommit?.Invoke(TypedNativeView.Text);
        }
        
        public static void MapTextProperty(IViewHandler viewHandler, TextField virtualView)
        {
            var nativeView = (WPFTextField)viewHandler.NativeView;
            nativeView.Text = virtualView.Text;
            //nativeView.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            //var desiredSize = nativeView.DesiredSize;
        }
    }
}
