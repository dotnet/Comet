using System;
using System.Diagnostics;
using Xamarin.Forms;
using FView = Xamarin.Forms.View;
using HView = HotUI.View;

namespace HotUI.Forms
{
	public class ViewHandler : FView, IFormsView
    {
        public static readonly PropertyMapper<View> Mapper = new PropertyMapper<View>()
        {

        };

        private View _view;

        public Action ViewChanged { get; set; }

        public FView View { get; private set; }
        public object NativeView => View;
        public bool HasContainer { get; set; } = false;

        public void Remove(View view)
        {
            _view = null;
            View = null;
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
            Mapper.UpdateProperties(this, _view);
        }

        public void SetBody()
        {
            var formsView = _view?.ToIFormsView();
            if (formsView?.GetType() == typeof(ViewHandler) && _view.Body == null)
            {
                // this is recursive.
                Debug.WriteLine($"There is no ViewHandler for {_view.GetType()}");
            }

            View = formsView?.View ?? new Xamarin.Forms.ContentView();
        }
    }
}
