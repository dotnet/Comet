using Android.Widget;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public class TextHandler : TextView, IView
    {
        public AView View => this;
        public object NativeView => View;
        public bool HasContainer { get; set; } = false;

        public TextHandler() : base(AndroidContext.CurrentContext)
        {
            
        }

        public void Remove(View view)
        {
        }

        public void SetView(View view)
        {
            var label = view as Text;
            this.UpdateLabelProperties(label);
        }

        public void UpdateValue(string property, object value)
        {
            this.UpdateLabelProperty(property, value);
        }
    }

    public static partial class ControlExtensions
    {
        public static void UpdateLabelProperties(this TextView view, Text hView)
        {
            view.Text = hView?.Value;
            view.UpdateBaseProperties(hView);
        }

        public static bool UpdateLabelProperty(this TextView view, string property, object value)
        {
            switch (property)
            {
                case nameof(Text.Value):
                    view.Text = (string) value;
                    return true;
            }

            return view.UpdateBaseProperty(property, value);
        }
    }
}