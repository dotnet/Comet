using System;
namespace Comet
{
	public class WebView : View
	{
		Binding<string> html;
		public Binding<string> Html
		{
			get => html;
			set => this.SetBindingValue(ref html, value);
		}

		Binding<string> source;
		public Binding<string> Source
		{
			get => source;
			set => this.SetBindingValue(ref source, value);
		}


	}
}
