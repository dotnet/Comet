using Comet.Internal;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.HotReload;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Comet.Tests
{
	public class TestBase
	{
		public TestBase()
		{
			UI.Init();
		}

		public static void InitializeHandlers(View view)
		{
			if (view == null) return;

			var handler = view.ViewHandler;
			if (handler == null)
			{
				var v = view.ReplacedView;
				handler = UI.Handlers.GetHandler(view.GetType());
				view.ViewHandler = handler;
				handler.SetVirtualView(view);

			}

			if (view is AbstractLayout layout)
			{
				foreach (var subView in layout)
				{
					InitializeHandlers(subView);
				}
			}
			else if (view is ContentView contentView)
			{
				InitializeHandlers(contentView.Content);
			}
			else if (view.BuiltView != null)
			{
				InitializeHandlers(view.BuiltView);
			}

		}

		public static void InitializeHandlers(View view, float width, float height)
		{
			InitializeHandlers(view);
			view.Alignment(Alignment.Fill);
			var frame = new Rect(0, 0, width, height);
			var iView = (IView)view;
			iView.Measure(frame.Width, frame.Height);
			iView.Arrange(frame);
		}
		
		public static void ResetComet()
		{
			var v = new View();
			v.ResetGlobalEnvironment();
			//v.DisposeAllViews();
			UI.Init(true);
			MauiHotReloadHelper.Reset();
			v?.Dispose();
		}
	}
}
