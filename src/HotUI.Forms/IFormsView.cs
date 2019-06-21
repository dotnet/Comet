using System;
namespace HotUI.Forms {
	public interface IFormsView {
		Xamarin.Forms.View View { get; }
	}
	public interface IFormsPage {
		Xamarin.Forms.Page Page { get; }
	}
}
