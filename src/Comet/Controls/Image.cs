using System;
using Comet.Graphics;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class Image : View, Microsoft.Maui.IImage
	{
		public Image(Binding<IImageSource> imageSource = null)
		{
			ImageSource = imageSource;
		}

		public Image(Binding<string> source)
		{
			Source = source;
		}

		public Image(Func<IImageSource> bitmap) : this((Binding<IImageSource>)bitmap) { }

		public Image(Func<string> source) : this((Binding<string>)source) { }

		private Binding<IImageSource> _imageSource;
		public Binding<IImageSource> ImageSource
		{
			get => _imageSource;
			private set => this.SetBindingValue(ref _imageSource, value);
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

		private void CreateImageSource(string source)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(source))
				{
					ImageSource = null;
					return;
				}
				ImageSource = (ImageSource)source;
			}
			catch (Exception exc)
			{
				Logger.Warn("An unexpected error occurred loading a bitmap.", exc);
			}
		}

		void IImageSourcePart.UpdateIsLoading(bool isLoading)
		{

		}

		Aspect Microsoft.Maui.IImage.Aspect => this.GetEnvironment<Aspect?>(nameof(Aspect)) ?? Aspect.AspectFill;

		bool Microsoft.Maui.IImage.IsOpaque => this.GetEnvironment<bool>(nameof(Microsoft.Maui.IImage.IsOpaque));

		IImageSource IImageSourcePart.Source => ImageSource?.CurrentValue;

		bool IImageSourcePart.IsAnimationPlaying => this.GetEnvironment<bool>(nameof(Microsoft.Maui.IImage.IsAnimationPlaying));
	}
}
