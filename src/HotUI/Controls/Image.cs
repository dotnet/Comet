using System;
namespace HotUI {
	public class Image : View {
		public Image () { }
		public Image(string source)
		{
			Source = source;
		}
		string source;
		public string Source {
			get => source;
			set => this.SetValue (State, ref source, value, ViewPropertyChanged);
		}
		
	}
}
