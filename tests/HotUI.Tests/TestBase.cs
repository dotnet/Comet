using System;
using Xunit;

[assembly: CollectionBehavior (DisableTestParallelization = true)]
namespace HotUI.Tests 
{
	public class TestBase 
	{
		public TestBase ()
		{
			UI.Init ();
		}
		
		public static void InitializeHandlers(View view)
		{
			if (view == null) return;
			
			var handler = view.ViewHandler;
			if (handler == null) 
			{
				handler = Registrar.Handlers.GetRenderer (view.GetType ()) as IViewHandler;
				view.ViewHandler = handler;
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
	}
}
