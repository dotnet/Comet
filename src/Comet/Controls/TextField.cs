using System;
using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class TextField : View, IEntry
	{
		protected static Dictionary<string, string> TextHandlerPropertyMapper = new(HandlerPropertyMapper)
		{
			[nameof(Color)] = nameof(IText.TextColor),
		};
		public TextField(
			Binding<string> value = null,
			string placeholder = null,
			Action<string> onEditingChanged = null,
			Action<string> onCommit = null)
		{
			Text = value;
			Placeholder = placeholder;
			OnEditingChanged = new MulticastAction<string>(Text, onEditingChanged);
			OnCommit = onCommit;
		}
		public TextField(
			Func<string> value,
			string placeholder = null,
			Action<string> onEditingChanged = null,
			Action<string> onCommit = null) : this((Binding<string>)value, placeholder, onEditingChanged, onCommit)
		{

		}



		Binding<string> _text;
		public Binding<string> Text
		{
			get => _text;
			private set => this.SetBindingValue(ref _text, value);
		}

		Binding<string> _placeholder;
		public Binding<string> Placeholder
		{
			get => _placeholder;
			private set => this.SetBindingValue(ref _placeholder, value);
		}

		public Action<TextField> Focused { get; private set; }
		public Action<TextField> Unfocused { get; private set; }
		public Action<string> OnEditingChanged { get; private set; }
		public Action<string> OnCommit { get; private set; }

		bool IEntry.IsPassword => false;

		//TODO: Expose these properties
		bool IEntry.IsTextPredictionEnabled => this.GetEnvironment<bool>(nameof(IEntry.IsTextPredictionEnabled));

		ReturnType IEntry.ReturnType => this.GetEnvironment<ReturnType>(nameof(IEntry.ReturnType));

		ClearButtonVisibility IEntry.ClearButtonVisibility => this.GetEnvironment<ClearButtonVisibility>(nameof(IEntry.ClearButtonVisibility));

		string ITextInput.Text {
			get => Text;
			set => Text.Set(value);
		}

		bool ITextInput.IsReadOnly => this.GetEnvironment<bool>(nameof(IEntry.IsReadOnly));

		int ITextInput.MaxLength => this.GetEnvironment<int?>(nameof(IEntry.MaxLength)) ?? -1;

		string IText.Text => Text;

		Color IText.TextColor => this.GetColor();

		Font IText.Font => this.GetFont(null);

		double IText.CharacterSpacing => this.GetEnvironment<double>(nameof(IText.CharacterSpacing));

		string IPlaceholder.Placeholder => this.Placeholder;

		TextAlignment ITextAlignment.HorizontalTextAlignment => this.GetTextAlignment() ?? TextAlignment.Start;

		public void ValueChanged(string value)
			=> OnEditingChanged?.Invoke(value);

		protected override string GetHandlerPropertyName(string property)
			=> TextHandlerPropertyMapper.TryGetValue(property, out var value) ? value : property;
	}
}
