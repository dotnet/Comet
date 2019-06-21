using System;
using FControlType = Xamarin.Forms.WebView;
namespace HotForms {
	public class WebView : View {
		string html;
		public string Html {
			get => html;
			set => this.SetValue (State, ref html, value, ViewPropertyChanged);
		}

		string source;
		public string Source {
			get => source;
			set => this.SetValue (State, ref source, value, ViewPropertyChanged);
		}

		
	}
}
