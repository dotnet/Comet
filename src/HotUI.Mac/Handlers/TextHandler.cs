using System;
using System.Collections.Generic;
using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers
{
    public class TextHandler : NSTextField, INSView
    {
        private static readonly PropertyMapper<Text, NSTextField> Mapper = new PropertyMapper<Text, NSTextField>()
        {
            [nameof(Text.Value)] = MapValueProperty
        };
        
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

        private Text _text;
        
        public void SetView(View view)
        {
            _text = view as Text;
            Mapper.UpdateProperties(this, _text);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _text, property);
        }
        
        public static bool MapValueProperty(NSTextField nativeView, Text virtualView)
        {
            nativeView.StringValue = virtualView.Value;
            nativeView.SizeToFit();
            return true;
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