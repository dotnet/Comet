using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

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

		string IText.Text => _text?.CurrentValue;

		Font IText.Font => Font.Default;


		double IText.CharacterSpacing => this.GetEnvironment<double>(nameof(IText.CharacterSpacing));

		Color IText.TextColor => this.GetColor();

		Thickness IPadding.Padding => this.GetPadding();

		void IButton.Pressed() { }
		void IButton.Released() { }
		void IButton.Clicked() => OnClick?.Invoke();
	}
}
