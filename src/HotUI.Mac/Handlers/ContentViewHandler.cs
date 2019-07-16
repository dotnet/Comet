using System;
using AppKit;
using HotUI.Mac.Controls;
using HotUI.Mac.Extensions;

namespace HotUI.Mac {
	public class ContentViewHandler : MacViewHandler {
		public ContentViewHandler ()
		{
		}

		public NSView View => ContentView?.Content.ToView ();

		public object NativeView => View;
		public bool HasContainer { get; set; } = false;
		public HUIContainerView ContainerView => null;

		public void Remove (View view)
		{
			ContentView = null;
		}

		ContentView ContentView;
		public void SetView (View view)
		{
			ContentView = view as ContentView;
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
            //TODO: Clean up once it is updated
            //_view?.RemoveFromSuperview();
            //_view?.Dispose();
            //_view = null;
            //if (_contentView != null)
            //    Remove(_contentView);
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
