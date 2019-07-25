using HotUI.Blazor.Components;
using System;

namespace HotUI.Blazor.Handlers
{
    internal class TextFieldHandler : AbstractControlHandler<TextField, BTextField>
    {
        public static PropertyMapper<TextField> Mapper = new PropertyMapper<TextField>
        {
            [nameof(TextField.Text)] = MapValueProperty
        };

        public TextFieldHandler()
            : base(Mapper)
        {
            
        }

        protected override BTextField CreateView() => new BTextField();

        protected override void InitializeView()
        {
            NativeView.TextChanged = HandleOnChange;
            NativeView.TextInput = HandleOnInput;

        }

        public override void Remove(View view)
        {
            NativeView.TextChanged = null;
            NativeView.TextInput = null;
            base.Remove(view);
        }

        private void HandleOnInput(string value)
        {
            VirtualView?.OnEditingChanged?.Invoke(value);
        }

        private void HandleOnChange(string value)
        {
            VirtualView?.OnCommit?.Invoke(value);
        }

        public static void MapValueProperty(IViewHandler viewHandler, TextField virtualView)
        {
            var nativeView = (BTextField)viewHandler.NativeView;
            nativeView.Text = virtualView.Text;
            virtualView.InvalidateMeasurement();
        }
    }
}
