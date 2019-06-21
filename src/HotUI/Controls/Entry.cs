using System;
namespace HotUI {
	public class Entry : View {

		string text;
		public string Text {
			get => text;
			set => this.SetValue (State, ref text, value, ViewPropertyChanged);
		}

		string placeholder;
		public string Placeholder {
			get => placeholder;
			set => this.SetValue (State, ref placeholder, value, ViewPropertyChanged);
		}

		public Action<string> Completed { get; set; }
		public Action<Entry> Focused { get; set; }
		public Action<Entry> Unfocused { get; set; }
		public Action<(string NewText, string OldText)> TextChanged { get; set; }
	}
}
