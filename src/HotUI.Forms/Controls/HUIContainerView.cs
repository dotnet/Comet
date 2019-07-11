using System;
using FGrid = Xamarin.Forms.Grid; 
using FView = Xamarin.Forms.View;

namespace HotUI.Forms
{
    public class HUIContainerView : FGrid
    {
        private FView _mainView;

        public HUIContainerView()
        {
        }

        public FView MainView
        {
            get => _mainView;
            internal set
            {
                if (_mainView == value)
                    return;

                if (_mainView != null)
                {
                    Children.Remove(_mainView);                    
                }

                _mainView = value;

                if (_mainView != null)
                {
                    FGrid.SetRow(_mainView, 0);
                    FGrid.SetColumn(_mainView, 0);
                    FGrid.SetRowSpan(_mainView, 1);
                    FGrid.SetColumnSpan(_mainView, 1);
                    Children.Add(_mainView);
                }
            }
        }
    }
}
