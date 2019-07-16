using System;
using AView = global::Android.Views.View;
using HotUI.Android.Controls;
using LP = Android.Views.ViewGroup.LayoutParams;

namespace HotUI.Android.Handlers 
{
	public class ContentViewHandler: CustomFrameLayout, AndroidViewHandler
	{
		private AView _view;
		private ContentView _contentView;

		public ContentViewHandler () : base(AndroidContext.CurrentContext)
		{

		}

		public AView View => this;
		
		public object NativeView => View;

		public bool HasContainer
		{
			get => false;
			set { }
		} 
		
		public void Remove (View view)
		{
            if(!isDisposed)
                this.RemoveAllViews();
			_view = null;
			_contentView = null;
		}

		public void SetView (View view)
		{
			_contentView = view as ContentView;
			_view = _contentView?.Content?.ToView ();
            AddView(_view, new LP(LP.MatchParent, LP.MatchParent));
        }

		public void UpdateValue (string property, object value)
		{

		}

        bool isDisposed;
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if (_contentView != null)
                {
                    _contentView.ViewHandler = null;
                }
                _contentView = null;
                _view?.Dispose();
            }
            isDisposed = true;
            base.Dispose(disposing);
        }
    }
}
