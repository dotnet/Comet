using System;
namespace HotUI.Forms {
	public interface IFormsView  : IViewHandler{
		Xamarin.Forms.View View { get; }
	}
	public interface IFormsPage : IViewBuilderHandler {
		Xamarin.Forms.Page Page { get; }
	}
}
