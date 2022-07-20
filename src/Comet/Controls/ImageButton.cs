using System;
using System.Collections.Generic;
using Comet.Graphics;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public partial class ImageButton : View, Microsoft.Maui.IImageButton
	{
		protected static Dictionary<string, string> ImageHandlerPropertyMapper = new(HandlerPropertyMapper)
		{
			[nameof(ImageSource)] = nameof(IImageSourcePart.Source),
		};

		public ImageButton(Binding<string> source, Action clicked = null)
		{
			StringSource = source;
			Clicked = clicked;
		}

		public ImageButton(Func<string> source, Action clicked = null) : this((Binding<string>)source, clicked) { }

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
				InvalidateMeasurement();
				CreateImageSource((string)value);
			}
		}

		private void CreateImageSource(string source)
		{
			try
			{
				this.source ??= new Binding<IImageSource>();
				this.source.Set((ImageSource)source);
				ViewHandler?.UpdateValue(nameof(IImageSourcePart.Source));
			}
			catch (Exception exc)
			{
				Logger.Warn("An unexpected error occurred loading a bitmap.", exc);
			}
		}

		protected override string GetHandlerPropertyName(string property)
			=> ImageHandlerPropertyMapper.TryGetValue(property, out var value) ? value : property;
	}
}
