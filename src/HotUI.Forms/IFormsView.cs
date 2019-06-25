using System;
namespace HotUI.Forms {
	public interface IFormsView  : IViewHandler{
		Xamarin.Forms.View View { get; }
	}
}
