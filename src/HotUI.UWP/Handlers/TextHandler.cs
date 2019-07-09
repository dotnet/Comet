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
        public static readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>()
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

        public object NativeView => View;

        public bool HasContainer
        {
            get => false;
            set { }
        }
        public void Remove(View view)
        {
        }

        public void SetView(View view)
        {
            _text = view as Text;
            Mapper.UpdateProperties(this, _text);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _text, property);
        }

        public static bool MapValueProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (UWPLabel)viewHandler.NativeView;
            nativeView.Text = virtualView.Value;
            return true;
        }
    }
}