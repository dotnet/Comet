using System;

namespace Comet
{
	public class NavigationView : ContentView, IStackNavigationView, IToolbarElement
	{
		List<IView> _views = new List<IView>();
		public void Navigate(View view)
		{
			view.Navigation = this;
			view.UpdateNavigation();

			if (PerformNavigate == null && Navigation != null)
				Navigation.Navigate(view);
			else
			{
				_views.Add(view);
				if (PerformNavigate != null)
					PerformNavigate(view);
				else
					((IStackNavigationView)this).RequestNavigation(new NavigationRequest(_views, true));
			}
		}

		public void SetPerformPop(Action action) => PerformPop = action;
		public void SetPerformPop(NavigationView navView)
			=> PerformPop = navView.PerformPop;
		protected Action PerformPop { get; set; }

		public void SetPerformNavigate(Action<View> action)
			=> PerformNavigate = action;
		public void SetPerformNavigate(NavigationView navView)
			=> PerformNavigate = navView.PerformNavigate;

		protected Action<View> PerformNavigate { get; set; }

		IToolbar IToolbarElement.Toolbar => CometWindow.Toolbar;

		protected override void OnHandlerChange()
		{
			if (_views.Count == 0 && Content != null)
				_views.Add(Content);

			((IStackNavigationView)this).RequestNavigation(new NavigationRequest(_views, false));
		}

		public void Pop()
		{
			if (PerformPop == null && Navigation != null)
				Navigation.Pop();
			else
				PerformPop();
		}

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

		public static void Pop(View view)
		{
			var parent = FindParentNavigationView(view);
			if (parent is ModalView)
			{
				ModalView.Dismiss();
			}
			else if (parent is NavigationView nav)
			{
				nav.Pop();
			}
		}

		//public static void PopToRoot(View view)
		//{

		//}

		//public static void PopToView(View fromView, View toView)
		//{

		//}

		static View FindParentNavigationView(View view)
		{
			if (view == null)
				return null;

			if (view.Parent is NavigationView || view.Parent is ModalView)
			{
				return view.Parent;
			}

			return FindParentNavigationView(view?.Parent) ?? view.Navigation;
		}

		void IStackNavigation.RequestNavigation(NavigationRequest eventArgs) =>
			ViewHandler?.Invoke(nameof(IStackNavigationView.RequestNavigation), eventArgs);
		void IStackNavigation.NavigationFinished(IReadOnlyList<IView> newStack) => _views = newStack.ToList();
	}
}
