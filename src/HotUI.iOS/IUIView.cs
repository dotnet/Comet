using System;
using UIKit;
namespace HotUI.iOS {
	public interface IUIView : IViewHandler {
		UIView View { get; }
	}
	public interface IUIViewController : IViewBuilderHandler {
		UIViewController ViewController { get; }
	}
}
