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
			this.Content = scroll.View?.ToForms ();
		}

		public void UpdateValue (string property, object value)
		{

		}
	}
}
