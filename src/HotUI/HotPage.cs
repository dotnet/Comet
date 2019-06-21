using System;

namespace HotUI {

	public abstract class HotPage : ViewBuilder {

		string title;
		public string Title {
			get => title;
			set => this.SetValue (ref title, value, ViewPropertyChanged);
		}


		public virtual void OnAppearing ()
		{
			
		}
		public virtual void OnDisppearing ()
		{

		}
	}
}
