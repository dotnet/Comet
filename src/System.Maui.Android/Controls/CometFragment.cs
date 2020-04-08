using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using AView = Android.Views.View;
using System.Maui.Internal;

namespace System.Maui.Android.Controls
{
	public class MauiFragment : Fragment
	{
		MauiView containerView;
		View startingCurrentView;

		public MauiFragment()
		{
		}

		public MauiFragment(View view)
		{
			this.CurrentView = view;
		}

		public string Title { get; set; }

		public View CurrentView
		{
			get => containerView?.CurrentView ?? startingCurrentView;
			set
			{
				if (containerView != null)
					containerView.CurrentView = value;
				else
					startingCurrentView = value;

				Title = value?.GetEnvironment<string>(EnvironmentKeys.View.Title) ?? value?.BuiltView?.GetEnvironment<string>(EnvironmentKeys.View.Title) ?? "";
			}
		}

		public override AView OnCreateView(LayoutInflater inflater,
			ViewGroup container,
			Bundle savedInstanceState)
		{
			if (CurrentView == null && savedInstanceState != null)
			{
				var oldViewId = savedInstanceState.GetString(currentViewID);
				var oldView = System.Maui.Internal.Extensions.FindViewById(null, oldViewId);
				startingCurrentView = oldView;
			}

			containerView ??= new MauiView(AndroidContext.CurrentContext);
			containerView.CurrentView = startingCurrentView;
			return containerView;
		}

		const string currentViewID = nameof(currentViewID);
		public override void OnSaveInstanceState(Bundle outState)
		{
			if (CurrentView != null)
			{
				string viewId = CurrentView.Id;
				outState.PutString(currentViewID, viewId);
			}

			base.OnSaveInstanceState(outState);
		}

		public override void OnDestroy()
		{
			if (containerView != null && containerView.CurrentView != null)
			{
				containerView.CurrentView.ViewHandler = null;
				containerView.CurrentView.Dispose();
				containerView.CurrentView = null;
			}

			base.OnDestroy();
			this.Dispose();
		}
	}
}
