using System;
namespace Comet
{
	public class NavigationView : ContentView
	{
		public void Navigate(View view)
		{
			view.Navigation = this;
			view.UpdateNavigation();
			PerformNavigate(view);
		}

		public Action<View> PerformNavigate { get; set; }

		public override void Add(View view)
		{
			base.Add(view);
			if (view != null)
			{
				view.Navigation = this;
				view.Parent = this;
			}
		}

		public static void Navigate(View fromView, View view)
		{
			if (view is ModalView modal)
			{
				ModalView.Present(modal.Content);
			}
			else if (fromView.Navigation != null)
			{
				fromView.Navigation.Navigate(view);
			}
			else
			{
				ModalView.Present(view);
			}
		}
	}
}
