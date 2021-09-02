using System;
using System.Collections.Generic;
using Microsoft.Maui;

namespace Comet
{
	public class NavigationView : ContentView, INavigationView
	{
		List<IView> stack = new List<IView>();
		public IReadOnlyList<IView> Stack => stack;
		public void Navigate(View view, bool animated = true)
		{
			view.Navigation = this;
			view.UpdateNavigation();
			stack.Add(view);
			this.ViewHandler?.Invoke(nameof(INavigationView.RequestNavigation), new NavigationRequest(Stack, animated));
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


		public void Pop(bool animated = true)
		{
			stack.RemoveAt(stack.Count - 1);

			this.ViewHandler?.Invoke(nameof(INavigationView.RequestNavigation), new NavigationRequest(Stack, animated));
		}

		public override void Add(View view)
		{
			base.Add(view);
			if (view != null)
			{
				view.Navigation = this;
				view.Parent = this;
			}
			stack.Add(view);
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

		protected override void OnHandlerSet()
		{
			base.OnHandlerSet();
			this.ViewHandler.Invoke(nameof(INavigationView.RequestNavigation), new NavigationRequest(Stack,false));
		}

		void INavigationView.RequestNavigation(NavigationRequest eventArgs) 
		{

		}
		void INavigationView.NavigationFinished(IReadOnlyList<IView> newStack) {
			stack.Clear();
			stack.AddRange(newStack);
		}
	}
}
