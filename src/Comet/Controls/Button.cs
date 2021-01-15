using System;
using System.Graphics;
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


		//TODO:Audit and fill these out.

		string IText.Text => _text?.CurrentValue;

		Color IText.Color => this.GetColor(null);

		Font IText.Font => Font.Default;

		TextTransform IText.TextTransform => TextTransform.Default;

		float IText.CharacterSpacing => this.GetEnvironment<float>(nameof(IText.CharacterSpacing));

		Xamarin.Forms.FontAttributes IFont.FontAttributes => throw new NotImplementedException();

		string IFont.FontFamily => null;

		float IFont.FontSize => this.GetEnvironment<float>(nameof(IText.FontSize));

		Xamarin.Forms.TextAlignment ITextAlignment.HorizontalTextAlignment => this.GetTextAlignment() ?? default;

		Xamarin.Forms.TextAlignment ITextAlignment.VerticalTextAlignment => this.GetTextAlignment() ?? default;

		void IButton.Pressed() { }
		void IButton.Released() { }
		void IButton.Clicked() => OnClick?.Invoke();
	}
}
