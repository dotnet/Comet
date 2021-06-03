using System;
using Android.OS;
using Android.Views;
using AView = Android.Views.View;
using Comet.Internal;
using AndroidX.Fragment.App;
using Microsoft.Maui;

namespace Comet.Android.Controls
{
	public class CometFragment : Fragment
	{
		CometView containerView;
		IView startingCurrentView;

		public IMauiContext MauiContext { get; set; }

		public CometFragment()
		{

		}

		public CometFragment(IMauiContext mauiContext)
		{
			MauiContext = mauiContext;
		}

		public CometFragment(View view, IMauiContext mauiContext) : this(mauiContext)
		{
			this.CurrentView = view;
		}

		public string Title { get; set; }

		public IView CurrentView
		{
			get => containerView?.CurrentView ?? startingCurrentView;
			set
			{
				if (containerView != null)
					containerView.CurrentView = value;
				else
					startingCurrentView = value;
				var view = value as View;
				Title = view?.GetEnvironment<string>(EnvironmentKeys.View.Title) ?? view?.BuiltView?.GetEnvironment<string>(EnvironmentKeys.View.Title) ?? "";
			}
		}

		public override AView OnCreateView(LayoutInflater inflater,
			ViewGroup container,
			Bundle savedInstanceState)
		{
			if (CurrentView == null && savedInstanceState != null)
			{
				var oldViewId = savedInstanceState.GetString(currentViewID);
				var oldView = Comet.Internal.Extensions.FindViewById(null, oldViewId);
				startingCurrentView = oldView;
				MauiContext = oldView.ViewHandler?.MauiContext;
			}

			containerView ??= new CometView(MauiContext);
			containerView.CurrentView = startingCurrentView;
			return containerView;
		}

		const string currentViewID = nameof(currentViewID);
		public override void OnSaveInstanceState(Bundle outState)
		{
			if (CurrentView != null)
			{
				string viewId = (CurrentView as View)?.Id;
				outState.PutString(currentViewID, viewId);
			}

			base.OnSaveInstanceState(outState);
		}

		public override void OnDestroy()
		{
			if (containerView != null && containerView.CurrentView != null)
			{
				//containerView.CurrentView.ViewHandler = null;
				//containerView.CurrentView.Dispose();
				containerView.CurrentView = null;
			}

			base.OnDestroy();
			this.Dispose();
		}
	}
}
