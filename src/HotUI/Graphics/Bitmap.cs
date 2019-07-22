using System;

namespace HotUI.Graphics
{
    public abstract class Bitmap : IDisposable
    {
        private bool _disposed;

        public abstract SizeF Size { get; }
        public abstract object NativeBitmap { get; }
        protected abstract void DisposeNative();

        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (NativeBitmap != null)
                DisposeNative();
        }

        void OnDispose(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;
            Dispose(disposing);
        }

        ~Bitmap()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            OnDispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
            // GC.SuppressFinalize(this);
        }
    }
}
