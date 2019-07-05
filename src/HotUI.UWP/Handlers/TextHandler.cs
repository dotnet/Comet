using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using UWPLabel = Windows.UI.Xaml.Controls.TextBlock;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.UWP.Handlers
{
    public class TextHandler : IUIElement
    {
        public static readonly PropertyMapper<Text, UIElement, UWPLabel> Mapper = new PropertyMapper<Text, UIElement, UWPLabel>()
            {
                [nameof(Text.Value)] = MapValueProperty
            };

        private Text _text;
        private readonly UWPLabel _textBlock;
        
        public TextHandler()
        {
            _textBlock = new UWPLabel();
        }
        
        public UIElement View => _textBlock;

        public void Remove(View view)
        {
        }

        public void SetView(View view)
        {
            _text = view as Text;
            Mapper.UpdateProperties(_textBlock, _text);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(_textBlock, _text, property);
        }

        public static bool MapValueProperty(UWPLabel nativeView, Text virtualView)
        {
            nativeView.Text = virtualView.Value;
            return true;
        }
    }
}