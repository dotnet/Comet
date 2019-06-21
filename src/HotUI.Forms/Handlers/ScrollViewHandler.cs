using System;
using HotUI;
using Xamarin.Forms;
using FScrollView = Xamarin.Forms.ScrollView;
using HScrollView = HotUI.ScrollView;
using HView = HotUI.View;
namespace HotUI.Forms {
	public class ScrollViewHandler : FScrollView, IViewHandler, IFormsView {
		public ScrollViewHandler ()
		{
		}

		public Xamarin.Forms.View View => this;

		public void Remove (HView view)
		{

		}

		public void SetView (HView view)
		{
			var scroll = view as HScrollView;
			var newContent = scroll.View?.ToForms ();
			if (Content == newContent)
				return;
			this.Content = newContent; 
		}

		public void UpdateValue (string property, object value)
		{

		}
	}
}
