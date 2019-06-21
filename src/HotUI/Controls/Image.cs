using System;
namespace HotUI {
	public class Image : View {
		string source;
		public string Source {
			get => source;
			set => this.SetValue (State, ref source, value, ViewPropertyChanged);
		}
		
	}
}
