using System;
using System.Graphics;
using Xamarin.Forms;
using Xamarin.Platform;

namespace Comet
{
	/// <summary>
	/// A view that displays one or more lines of read-only text.
	/// </summary>
	public class Text : View, ILabel
	{
		public Text(
			Binding<string> value = null)
		{
			Value = value;
		}
		public Text(
			Func<string> value) : this((Binding<string>)value)
		{

		}

		Binding<string> _value;
		public Binding<string> Value
		{
			get => _value;
			private set => this.SetBindingValue(ref _value, value);
		}
		public override string ToString() => _value?.Value?.ToString() ?? string.Empty;


		string IText.Text => Value?.CurrentValue;

		Color IText.Color => this.GetColor(null);

		Font IText.Font => throw new NotImplementedException();

		TextTransform IText.TextTransform => throw new NotImplementedException();

		float IText.CharacterSpacing => throw new NotImplementedException();

		Xamarin.Forms.FontAttributes IFont.FontAttributes => throw new NotImplementedException();

		string IFont.FontFamily => throw new NotImplementedException();

		float IFont.FontSize => throw new NotImplementedException();

		TextAlignment ITextAlignment.HorizontalTextAlignment => throw new NotImplementedException();

		TextAlignment ITextAlignment.VerticalTextAlignment => throw new NotImplementedException();
	}
}
