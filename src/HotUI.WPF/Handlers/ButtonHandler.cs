using System;
using System.Collections.Generic;
using System.Windows;
using WPFButton = System.Windows.Controls.Button;
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.WPF.Handlers
{
    public class ButtonHandler : WPFButton, WPFViewHandler
    {
        public static readonly PropertyMapper<Button> Mapper = new PropertyMapper<Button>()
        {
            [nameof(Button.Text)] = MapTextProperty
        };
        
        private Button _button;

        public ButtonHandler()
        {
            Click += HandleClick;
        }
        

        public UIElement View => this;

        public object NativeView => View;

        public bool HasContainer
        {
            get => false;
            set { }
        }

        public void Remove(View view)
        {
            _button = null;
        }

        public void SetView(View view)
        {
            _button = view as Button;
            /*RenderSize = new Size(100, 24);
            Width = RenderSize.Width;
            Height = RenderSize.Height;*/
            Mapper.UpdateProperties(this, _button);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _button, property);
        }

        private void HandleClick(object sender, EventArgs e) => _button?.OnClick();

        public static bool MapTextProperty(IViewHandler viewHandler, Button virtualButton)
        {
            var nativeButton = (WPFButton)viewHandler.NativeView;
            nativeButton.Content = virtualButton.Text;
            return true;
        }
    }
}