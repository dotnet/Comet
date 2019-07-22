using System;
using HotUI.Graphics;

namespace HotUI 
{
	public class Image : BoundControl
    {
        public Image(Binding<Bitmap> bitmap) : base(bitmap)
        {
            Bind(bitmap, nameof(Bitmap), value => Bitmap = (Bitmap)value);
        }

        public Image(Binding<string> source) : base(source)
        {
            Bind(source, nameof(Source), value => Source = (string)value);
        }
        
        public Image(Func<Bitmap> bitmap) : this ((Binding<Bitmap>)bitmap)
		{
            
		}
        
        public Image(Func<string> source) : this ((Binding<string>)source)
        {
            
        }

        private Bitmap _bitmap;
		public Bitmap Bitmap {
			get => _bitmap;
            private set => SetValue(ref _bitmap, value);
        }

        private string _source;
        public string Source {
            get => _source;
            private set
            {
                SetValue(ref _source, value);
                LoadBitmapFromSource(_source);
            }
        }
        
        private async void LoadBitmapFromSource(string source)
        {
            try
            {
                var loadBitmapTask = Device.BitmapService?.LoadBitmapAsync(source);
                if (loadBitmapTask != null)
                {
                    var bitmap = await loadBitmapTask;
                    Bitmap = bitmap;
                }
            }
            catch (Exception exc)
            {
                Logger.Warn("An unexpected error occurred loading a bitmap.", exc);
            }
        }
    }
}
