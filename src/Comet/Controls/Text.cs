using System;
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

		string IText.Text => Value?.CurrentValue;

		Xamarin.Forms.Color IText.Color => this.GetColor(default);

		Font IText.Font => default;

		TextTransform IText.TextTransform => default;

		double IText.CharacterSpacing => default;

		Xamarin.Forms.FontAttributes IFont.FontAttributes => default;

		string IFont.FontFamily => default;

		double IFont.FontSize => default;

		Xamarin.Forms.TextAlignment ITextAlignment.HorizontalTextAlignment => default;

		Xamarin.Forms.TextAlignment ITextAlignment.VerticalTextAlignment => default;

		public override string ToString() => _value?.Value?.ToString() ?? string.Empty;
	}
}
