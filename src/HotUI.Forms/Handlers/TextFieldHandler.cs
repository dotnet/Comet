using System;
using HotUI;
using Xamarin.Forms;
using FEntry = Xamarin.Forms.Entry;
using HView = HotUI.View;
namespace HotUI.Forms
{
    public class TextFieldHandler : AbstractHandler<TextField, FEntry>
    {
        public static readonly PropertyMapper<TextField> Mapper = new PropertyMapper<TextField>(ViewHandler.Mapper)
        {
            [nameof(TextField.Text)] = MapTextProperty
        };

        public TextFieldHandler() : base(Mapper)
        {
        }
        public static void MapTextProperty(IViewHandler viewHandler, TextField virtualView)
        {
            var nativeView = (FEntry)viewHandler.NativeView;
            nativeView.Text = virtualView.Text;
        }

        private void HandleCompleted(object sender, EventArgs e)
        {
            VirtualView?.OnCommit?.Invoke(TypedNativeView.Text);
        }

        private void HandleUnfocused(object sender, FocusEventArgs e)
        {
            VirtualView?.Unfocused?.Invoke(VirtualView);
        }

        private void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            VirtualView?.OnEditingChanged?.Invoke(e.NewTextValue);
        }

        private void HandleFocused(object sender, FocusEventArgs e)
        {
            VirtualView?.Focused?.Invoke(VirtualView);
        }

        protected override FEntry CreateView()
        {
            var entry = new FEntry();
            entry.Focused += HandleFocused;
            entry.TextChanged += HandleTextChanged;
            entry.Unfocused += HandleUnfocused;
            entry.Completed += HandleCompleted;
            return entry;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            var entry = TypedNativeView;
            if (entry != null)
            {
                entry.Focused -= HandleFocused;
                entry.TextChanged -= HandleTextChanged;
                entry.Unfocused -= HandleUnfocused;
                entry.Completed -= HandleCompleted;
            }
            base.Dispose(disposing);
        }

    }
}
