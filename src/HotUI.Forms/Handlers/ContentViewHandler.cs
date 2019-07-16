namespace HotUI.Forms.Handlers
{
    public class ContentViewHandler : FormsViewHandler
    {
        public ContentViewHandler()
        {
        }

        public Xamarin.Forms.View View => _contentView?.Content?.ToForms();
        public object NativeView => View;
        public bool HasContainer { get; set; } = false;

        ContentView _contentView;
        public void Remove(View view)
        {
            _contentView = null;
        }

        public void SetView(View view)
        {
            _contentView = view as ContentView;
        }

        public void UpdateValue(string property, object value)
        {

        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            //TODO: Clean up once it is updated
            //_view = null;
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

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            OnDispose(true);
        }
        #endregion
    }
}
