using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui;

namespace Comet.Android.Controls
{
	public class CometTabView : CustomFrameLayout
	{
		private readonly BottomNavigationView _bottomNavigationView;
		private List<CometFragment> _fragments;
		public IMauiContext MauiContext { get; set; }
		public CometTabView(IMauiContext context) : base(context.Context)
		{
			MauiContext = context;
			_bottomNavigationView = new BottomNavigationView(context.Context)
			{
				LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent)
				{
					Gravity = GravityFlags.Bottom
				}
			};

			var val = new TypedValue();
			context.Context.Theme.ResolveAttribute(global::Android.Resource.Attribute.ColorBackground, val, true);
			_bottomNavigationView.SetBackgroundColor(new global::Android.Graphics.Color(val.Data));

			_bottomNavigationView.ItemSelected += HandleNavigationItemSelected;

			AddView(_bottomNavigationView);
		}

		public void CreateTabs(IList<View> views)
		{
			_fragments = views.Select(v => new CometFragment(v,MauiContext )).ToList();
			_bottomNavigationView.Menu.Clear();

			if (views == null)
			{
				return;
			}

			for (int i = 0; i < views.Count(); i++)
			{
				var view = views[i];
				var fragment = new CometFragment(view,MauiContext);

				var title = view.GetEnvironment<string>(EnvironmentKeys.TabView.Title);
				var imagePath = view.GetEnvironment<string>(EnvironmentKeys.TabView.Image);

				_bottomNavigationView.Menu.Add(0, i, i, title);
			}

			var index = 0;
			(MauiContext.Context as MauiAppCompatActivity).SupportFragmentManager
				.BeginTransaction()
				.Add(Id, _fragments[index], index.ToString())
				.Show(_fragments[index])
				.Commit();
		}

		private void HandleNavigationItemSelected(object sender, Google.Android.Material.Navigation.NavigationBarView.ItemSelectedEventArgs e)
		{
			var index = e.Item.ItemId;
			var manager = (MauiContext.Context as MauiAppCompatActivity).SupportFragmentManager;
			var transaction = manager.BeginTransaction();

			if (manager.FindFragmentByTag(index.ToString()) == null)
			{
				transaction.Add(Id, _fragments[index], index.ToString());
			}

			transaction.Hide(_fragments[_bottomNavigationView.SelectedItemId]);
			transaction.Show(_fragments[index]);
			transaction.Commit();
		}
	}
}
