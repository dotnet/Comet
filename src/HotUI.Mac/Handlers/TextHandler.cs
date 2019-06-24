using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers
{
    public class TextHandler : NSTextField, INSView
    {
        public NSView View => this;

        public TextHandler()
        {
            Editable = false;
            Bezeled = false;
            DrawsBackground = false;
            Selectable = false;
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
        public static void UpdateLabelProperties(this NSTextField view, Text hView)
        {
            view.UpdateLabelProperty(nameof(Text.Value), hView?.Value);
            view.UpdateBaseProperties(hView);
        }

        public static bool UpdateLabelProperty(this NSTextField view, string property, object value)
        {
            switch (property)
            {
                case nameof(Text.Value):
                    view.StringValue = (string) value;
                    view.SizeToFit();
                    return true;
            }

            return view.UpdateBaseProperty(property, value);
        }
    }
}