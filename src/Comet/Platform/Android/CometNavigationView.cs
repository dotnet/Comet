using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using Microsoft.Maui;

namespace Comet.Android.Controls
{
	public class CometNavigationView : CustomFrameLayout
	{
		IMauiContext MauiContext { get; set; }
		public CometNavigationView(IMauiContext context) : base(context.Context)
		{
			MauiContext = context;
		}

		public void SetRoot(View view)
		{
			(MauiContext.Context).GetFragmentManager()
				.BeginTransaction()
				.Replace(Id, new CometFragment(view, MauiContext))
				.CommitAllowingStateLoss();
		}
		 
		public void NavigateTo(View view)
		{
			(MauiContext.Context).GetFragmentManager()
				.BeginTransaction()
				.SetTransition((int)global::Android.App.FragmentTransit.FragmentFade)
				.AddToBackStack(view.Id)
				.Replace(Id, new CometFragment(view, MauiContext))
				.CommitAllowingStateLoss();
		}

		public void Pop() =>
			(MauiContext.Context).GetFragmentManager().PopBackStack();
	}
}
