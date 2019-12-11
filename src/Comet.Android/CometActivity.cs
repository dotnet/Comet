using System.Linq;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Comet.Android.Controls;
using static Android.Support.V4.App.FragmentManager;

namespace Comet.Android
{
	public abstract class CometActivity : AppCompatActivity, IOnBackStackChangedListener

	{
		private View _page;

		public View Page
		{
			get => _page;
			set
			{
				_page = value;
				SetContentView(_page?.ToView());

				if (SupportActionBar != null)
				{
					SupportActionBar.Title = value?.GetEnvironment<string>(EnvironmentKeys.View.Title) ?? "";
				}
			}
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			AndroidContext.CurrentContext = this;
			UI.Init();
			Xamarin.Essentials.Platform.Init(this, savedInstanceState);

			SupportFragmentManager.AddOnBackStackChangedListener(this);
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		public override bool OnSupportNavigateUp()
		{
			if (SupportFragmentManager.BackStackEntryCount > 0)
			{
				SupportFragmentManager.PopBackStack();
			}

			return base.OnSupportNavigateUp();
		}

		public void OnBackStackChanged()
		{
			SupportActionBar?.SetDisplayHomeAsUpEnabled(SupportFragmentManager.BackStackEntryCount > 0);

			if (SupportFragmentManager.Fragments.Last() is CometFragment fragment && SupportActionBar != null)
			{
				SupportActionBar.Title = fragment.Title;
			}
		}
	}
}
