using System;
using Comet.iOS;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using UIKit;

namespace Comet.Handlers
{
	public partial class CometViewHandler : ViewHandler<View, CometView>, INativeViewHandler
	{
		public static PropertyMapper<View, CometViewHandler> CometViewMapper = new ()
		{
			[nameof(ITitledElement.Title)] = MapTitle,
			[nameof(IView.Background)] = MapBackgroundColor,
		};


		public CometViewHandler() : base(CometViewMapper)
		{

		}
		CometViewController viewController;
		UIViewController INativeViewHandler.ViewController => viewController ??= new CometViewController { ContainerView = this.NativeView, MauiContext = MauiContext };
		protected override CometView CreateNativeView() => new CometView(MauiContext);
		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);
			NativeView.CurrentView = view;
		}


		public static void MapTitle(CometViewHandler handler, View view)
		{
			var vc = handler?.viewController;
			if (vc == null)
				return;
			vc.Title = view.GetTitle() ?? "";
		}
		public static void MapBackgroundColor(CometViewHandler handler, View view)
		{
			var vc = handler?.viewController;
			if (vc == null)
				return;
			vc.View.UpdateBackground(view);
		}

	}
}
