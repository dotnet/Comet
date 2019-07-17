using System;
namespace HotUI {
	public class Image : Control {
		public Image () { }
		public Image(string source) : base (true)
		{
			Source = source;
		}
		string source;
		public string Source {
			get => source;
			private set => this.SetValue (State, ref source, value, ViewPropertyChanged);
		}
		
	}
}
