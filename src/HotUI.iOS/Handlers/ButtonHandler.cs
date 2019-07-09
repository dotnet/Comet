using System;
using System.Collections.Generic;
using HotUI.iOS.Controls;
using UIKit;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HotUI.iOS
{
    public class ButtonHandler : UIButton, iOSViewHandler
    {
        public static readonly PropertyMapper<Button> Mapper = new PropertyMapper<Button>(ViewHandler.Mapper)
        {
            [nameof(Button.Text)] = MapTextProperty
        };

        private Button _button;

        public ButtonHandler()
        {
            TouchUpInside += HandleTouchUpInside;
            SetTitleColor(UIColor.Blue, UIControlState.Normal);
            Layer.BorderColor = UIColor.Blue.CGColor;
            Layer.BorderWidth = .5f;
            Layer.CornerRadius = 3f;
        }

        public UIView View => this;

        public HUIContainerView ContainerView => null;

        public object NativeView => this;

        public bool HasContainer { get; set; } = false;

        public void Remove(View view)
        {
            _button = null;
        }

        public void SetView(View view)
        {
            _button = view as Button;
            Mapper.UpdateProperties(this, _button);
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _button, property);
        }

        private void HandleTouchUpInside(object sender, EventArgs e)
        {
            _button?.OnClick();
        }

        public static bool MapTextProperty(IViewHandler viewHandler, Button virtualView)
        {
            var nativeView = (UIButton) viewHandler.NativeView;
            nativeView.SetTitle(virtualView.Text, UIControlState.Normal);
            nativeView.SizeToFit();
            return true;
        }
    }
}