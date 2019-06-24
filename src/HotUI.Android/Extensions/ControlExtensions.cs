using AView = Android.Views.View;

namespace HotUI.Android
{
    public static partial class ControlExtensions
    {
        public static void UpdateProperties(this AView view, View hView)
        {
        }

        public static void UpdateBaseProperties(this AView view, View hView)
        {
            view.UpdateProperties(hView);
        }

        public static bool UpdateProperty(this AView view, string property, object value)
        {
            return false;
        }

        public static bool UpdateBaseProperty(this AView view, string property, object value) => view.UpdateProperty(property, value);
    }
}