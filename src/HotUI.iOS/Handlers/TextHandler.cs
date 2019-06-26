using System;
using UIKit;

namespace HotUI.iOS
{
    public class TextHandler : UILabel, IUIView
    {
        public UIView View => this;

        public void Remove(View view)
        {
        }

        public void SetView(View view)
        {
            var label = view as Text;
            this.UpdateProperties(label);
        }

        public void UpdateValue(string property, object value)
        {
            this.UpdateProperty(property, value);
        }
    }

    public static partial class ControlExtensions
    {
        public static void UpdateProperties(this UILabel view, Text hView)
        {
            view.UpdateProperty(nameof(Text.Value), hView?.Value);
            view.UpdateBaseProperties(hView);
        }

        public static bool UpdateProperty(this UILabel view, string property, object value)
        {
            switch (property)
            {
                case nameof(Text.Value):
                    view.Text = (string) value;
                    view.SizeToFit();
                    return true;
            }

            return view.UpdateBaseProperty(property, value);
        }
    }
}