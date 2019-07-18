using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using UWPButton = Windows.UI.Xaml.Controls.Button;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace HotUI.UWP.Handlers
{
    public class ButtonHandler : AbstractControlHandler<Button,UWPButton>
    {
        public static readonly PropertyMapper<Button> Mapper = new PropertyMapper<Button>()
        {
            [nameof(Button.Text)] = MapTextProperty
        };

        public ButtonHandler() : base(Mapper)
        {
        }

        protected override UWPButton CreateView()
        {
            var button = new UWPButton();
            button.Click += HandleClick;
            return button;
        }

        protected override void DisposeView(UWPButton nativeView)
        {
            nativeView.Click -= HandleClick;
        }

        private void HandleClick(object sender, RoutedEventArgs e) => VirtualView?.OnClick();

        public static void MapTextProperty(IViewHandler viewHandler, Button virtualButton)
        {
            var nativeButton = (UWPButton)viewHandler.NativeView;
            nativeButton.Content = virtualButton.Text;
        }
    }
}