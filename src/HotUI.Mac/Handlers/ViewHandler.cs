using System;
using System.Diagnostics;
using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac
{
	public class ViewHandler : INSView
    {
        public static readonly PropertyMapper<View> Mapper = new PropertyMapper<View>()
        {
            [nameof(EnvironmentKeys.Colors.BackgroundColor)] = MapBackgroundColorProperty
        };

        private View _view;
        private NSView _body;

        public Action ViewChanged { get; set; }

        public NSView View => _body;

        public object NativeView => View;
        public bool HasContainer { get; set; } = false;

        public void Remove(View view)
        {
            _view = null;
            _body = null;
        }

        public void SetView(View view)
        {
            _view = view;
            SetBody();
            Mapper.UpdateProperties(this, _view);
            ViewChanged?.Invoke();
        }

        public void UpdateValue(string property, object value)
        {
            Mapper.UpdateProperty(this, _view, property);
        }

        public bool SetBody()
        {
            var uiElement = _view?.ToINSView();
            if (uiElement?.GetType() == typeof(ViewHandler) && _view.Body == null)
            {
                // this is recursive.
                Debug.WriteLine($"There is no ViewHandler for {_view.GetType()}");
                return true;
            }

            _body = uiElement?.View ?? new NSColorView();
            return true;
        }

        public static bool MapBackgroundColorProperty(IViewHandler viewHandler, View virtualView)
        {
            var nativeView = (NSView)viewHandler.NativeView;
            var color = virtualView.GetBackgroundColor();
            if (color != null)
            {
                if (nativeView is NSColorView colorView)
                {
                    colorView.BackgroundColor = color.ToNSColor();
                }
                else if (nativeView is NSTextField textField)
                {
                    textField.BackgroundColor = color.ToNSColor();
                    textField.DrawsBackground = true;
                }
            }

            return true;
        }
    }
}
