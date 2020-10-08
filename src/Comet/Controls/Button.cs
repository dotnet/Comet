using System;
using Xamarin.Forms;
using Xamarin.Platform;

namespace Comet
{
	public class Button : View, IButton
	{
		public Button(
			Binding<string> text = null,
			Action action = null)
		{
			Text = text;
			OnClick = action;
		}

		public Button(
			Func<string> text,
			Action action = null) : this((Binding<string>)text, action)
		{

		}

		Binding<string> _text;
		public Binding<string> Text
		{
			get => _text;
			private set => this.SetBindingValue(ref _text, value);
		}

		public Action OnClick { get; private set; }

		string IText.Text => Text?.CurrentValue;

		Xamarin.Forms.Color IText.Color => this.GetColor(Color.Black);

		Font IText.Font => default;

		TextTransform IText.TextTransform => default;

		double IText.CharacterSpacing => default;

		Xamarin.Forms.FontAttributes IFont.FontAttributes => default;

		string IFont.FontFamily => default;

		double IFont.FontSize => default;

		Xamarin.Forms.TextAlignment ITextAlignment.HorizontalTextAlignment => default;

		Xamarin.Forms.TextAlignment ITextAlignment.VerticalTextAlignment => default;

		void IButton.Pressed() {}
		void IButton.Released() {}
		void IButton.Clicked() => OnClick?.Invoke();
	}
}
