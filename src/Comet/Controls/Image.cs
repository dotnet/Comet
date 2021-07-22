using System;
using System.Collections.Generic;
using Comet.Graphics;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class Image : View, Microsoft.Maui.IImage
	{
		protected static Dictionary<string, string> ImageHandlerPropertyMapper = new(HandlerPropertyMapper)
		{
			[nameof(ImageSource)] = nameof(IImageSourcePart.Source),
		};
		public Image(Binding<IImageSource> imageSource = null)
		{
			ImageSource = imageSource;
		}

		public Image(Binding<string> source)
		{
			StringSource = source;
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
		public Binding<string> StringSource
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
			if (property == nameof(StringSource))
			{
				this.InvalidateMeasurement();
				CreateImageSource((string)value);
			}
		}

		private void CreateImageSource(string source)
		{
			try
			{
				_imageSource ??= new Binding<IImageSource>();
				_imageSource.Set((ImageSource)source);
				ViewHandler?.UpdateValue(nameof(IImageSourcePart.Source));
			}
			catch (Exception exc)
			{
				Logger.Warn("An unexpected error occurred loading a bitmap.", exc);
			}
		}

		void IImageSourcePart.UpdateIsLoading(bool isLoading)
		{

		}

		Aspect Microsoft.Maui.IImage.Aspect => this.GetEnvironment<Aspect>(nameof(Aspect));

		bool Microsoft.Maui.IImage.IsOpaque => this.GetEnvironment<bool>(nameof(Microsoft.Maui.IImage.IsOpaque));

		IImageSource IImageSourcePart.Source => ImageSource?.CurrentValue;

		bool IImageSourcePart.IsAnimationPlaying => this.GetEnvironment<bool>(nameof(Microsoft.Maui.IImage.IsAnimationPlaying));


		protected override string GetHandlerPropertyName(string property)
			=> ImageHandlerPropertyMapper.TryGetValue(property, out var value) ? value : property;
	}
}
