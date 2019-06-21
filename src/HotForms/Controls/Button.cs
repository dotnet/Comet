using System;
namespace HotForms {
	public class Button : View {

		private string text;
		public string Text {
			get => text;
			set => this.SetValue (State, ref text, value, ViewPropertyChanged);
		}

		public Action OnClick { get; set; }
		
	}
}
