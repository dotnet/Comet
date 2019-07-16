using System;
using AView = global::Android.Views.View;

namespace HotUI.Android.Handlers 
{
	public class ContentViewHandler: AndroidViewHandler
	{
		private AView _view;
		private ContentView _contentView;

		public ContentViewHandler ()
		{
		}

		public AView View => _view;
		
		public object NativeView => View;

		public bool HasContainer
		{
			get => false;
			set { }
		} 
		
		public void Remove (View view)
		{
			_view = null;
			_contentView = null;
		}

		public void SetView (View view)
		{
			_contentView = view as ContentView;
			_view = _contentView?.Content?.ToView ();
		}

		public void UpdateValue (string property, object value)
		{

		}


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            _view?.Dispose();
            _view = null;
            if (_contentView != null)
                Remove(_contentView);
        }

        void OnDispose(bool disposing)
        {
            if (disposedValue)
                return;
            disposedValue = true;
            Dispose(disposing);
        }

        ~ContentViewHandler()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            OnDispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            OnDispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
