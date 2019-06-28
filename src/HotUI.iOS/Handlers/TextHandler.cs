using System;
using System.Collections.Generic;
using UIKit;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.iOS
{
    public class TextHandler : UILabel, IUIView
    {
        private static readonly PropertyMapper<Text, TextHandler> Mapper = new PropertyMapper<Text, TextHandler>(new Dictionary<string, Func<TextHandler, Text, bool>>()
        {
            [nameof(HotUI.Text.Value)] = MapValueProperty
        });
        
        private Text _text;

        public UIView View => this;

        public void Remove(View view)
        {
            _text = null;
        }

        public void SetView(View view)
        {
            _text= view as Text;
            Mapper.UpdateProperties(this, _text);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _text, property);
        }
        
        public static bool MapValueProperty(TextHandler nativeView, Text virtualView)
        {
            nativeView.Text = virtualView.Value;
            nativeView.SizeToFit();
            return true;
        }
    }
}