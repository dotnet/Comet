using System;
using System.Drawing;
using Comet.Graphics;
namespace Comet
{
	public class Image : View
	{
		public Image(Binding<Bitmap> bitmap = null)
		{
			Bitmap = bitmap;
		}

		public Image(Binding<string> source)
		{
			Source = source;
		}

		public Image(Func<Bitmap> bitmap) : this((Binding<Bitmap>)bitmap) { }

		public Image(Func<string> source) : this((Binding<string>)source) { }

		private Binding<Bitmap> _bitmap;
		public Binding<Bitmap> Bitmap
		{
			get => _bitmap;
			private set => this.SetBindingValue(ref _bitmap, value);
		}

		private Binding<string> _source;
		public Binding<string> Source
		{
			get => _source;
			protected set
			{
				this.SetBindingValue(ref _source, value);
				LoadBitmapFromSource(_source.CurrentValue);
			}
		}

		public override void ViewPropertyChanged(string property, object value)
		{
			base.ViewPropertyChanged(property, value);
			if (property == nameof(Source))
			{
				LoadBitmapFromSource((string)value);
			}
		}

		public override SizeF GetIntrinsicSize(SizeF availableSize)
		{
			if (Bitmap?.Value != null)
				return Bitmap.GetValueOrDefault().Size;

			return SizeF.Empty;
		}

		private async void LoadBitmapFromSource(string source)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(source))
				{
					Bitmap = null;
					return;
				}
				var loadBitmapTask = Device.BitmapService?.LoadBitmapAsync(source);
				if (loadBitmapTask != null)
				{
					var bitmap = await loadBitmapTask;
					Bitmap = bitmap;
					this.ViewPropertyChanged(nameof(Bitmap), bitmap);
				}
			}
			catch (Exception exc)
			{
				Logger.Warn("An unexpected error occurred loading a bitmap.", exc);
			}
		}
	}
}
