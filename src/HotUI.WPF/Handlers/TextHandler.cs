using System;
using System.Collections.Generic;
using System.Windows;
using WPFLabel = System.Windows.Controls.Label;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.WPF.Handlers
{
    public class TextHandler : WPFLabel, WPFViewHandler
    {
        public static readonly PropertyMapper<Text> Mapper = new PropertyMapper<Text>()
            {
                [nameof(Text.Value)] = MapValueProperty
            };

        private Text _text;

        public UIElement View => this;

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
            /*RenderSize = new Size(100, 24);
            Width = RenderSize.Width;
            Height = RenderSize.Height;*/
            Mapper.UpdateProperties(this, _text);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _text, property);
        }

        public static bool MapValueProperty(IViewHandler viewHandler, Text virtualView)
        {
            var nativeView = (WPFLabel)viewHandler.NativeView;
            nativeView.Content = virtualView.Value;
            return true;
        }
    }
}