using System;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace System.Maui.Android.Controls
{
	public class MauiNavigationView : CustomFrameLayout
	{
		public MauiNavigationView(Context context) : base(context)
		{
		}

		public void SetRoot(View view)
		{
			AndroidContext.AppCompatActivity.SupportFragmentManager
				.BeginTransaction()
				.Replace(Id, new MauiFragment(view))
				.CommitAllowingStateLoss();
		}

		public void NavigateTo(View view)
		{
			AndroidContext.AppCompatActivity.SupportFragmentManager.BeginTransaction()
				.SetTransition((int)global::Android.App.FragmentTransit.FragmentFade)
				.AddToBackStack(view.Id)
				.Replace(Id, new MauiFragment(view))
				.CommitAllowingStateLoss();
		}

		public void Pop() =>
			AndroidContext.AppCompatActivity.SupportFragmentManager.PopBackStack();
	}
}
