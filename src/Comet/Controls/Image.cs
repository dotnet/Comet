using System;
using Comet.Graphics;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class Image : View, Microsoft.Maui.IImage
	{
		public Image(Binding<IImageSource> bitmap = null)
		{
			IImageSource = bitmap;
		}

		public Image(Binding<string> source)
		{
			Source = source;
		}

		public Image(Func<IImageSource> bitmap) : this((Binding<IImageSource>)bitmap) { }

		public Image(Func<string> source) : this((Binding<string>)source) { }

		private Binding<IImageSource> _bitmap;
		public Binding<IImageSource> IImageSource
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
				CreateImageSource(_source.CurrentValue);
			}
		}

		public override void ViewPropertyChanged(string property, object value)
		{
			base.ViewPropertyChanged(property, value);
			if (property == nameof(Source))
			{
				CreateImageSource((string)value);
			}
		}

		private async void CreateImageSource(string source)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(source))
				{
					IImageSource = null;
					return;
				}
				//var loadBitmapTask = Device.BitmapService?.LoadBitmapAsync(source);
				//if (loadBitmapTask != null)
				//{
				//	var bitmap = await loadBitmapTask;
				//	IImageSource = bitmap;
				//	this.ViewPropertyChanged(nameof(IImageSource), bitmap);
				//	this.InvalidateMeasurement();
				//}
			}
			catch (Exception exc)
			{
				Logger.Warn("An unexpected error occurred loading a bitmap.", exc);
			}
		}

		void IImageSourcePart.UpdateIsLoading(bool isLoading) => throw new NotImplementedException();

		Aspect Microsoft.Maui.IImage.Aspect => this.GetEnvironment<Aspect>(nameof(Aspect));

		bool Microsoft.Maui.IImage.IsOpaque => this.GetEnvironment<bool>(nameof(Microsoft.Maui.IImage.IsOpaque));

		IImageSource IImageSourcePart.Source => this.GetEnvironment<IImageSource>(nameof(IImageSourcePart.Source));

		bool IImageSourcePart.IsAnimationPlaying => this.GetEnvironment<bool>(nameof(Microsoft.Maui.IImage.IsAnimationPlaying));
	}
}
