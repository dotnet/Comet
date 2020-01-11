using System;
using System.Collections.Generic;
using System.Linq;

namespace Comet
{
	public interface IApplicationLifeCycle : IDisposable
	{
		void SetCurrentApp(CometApp app);
		void OnInit();
		void OnPause();
		void OnResume();
		void OnNavigate(Uri uri);
	}

	public class CometApp : View, IApplicationLifeCycle
	{
		public static CometApp CurrentApp { get; private set; }
		public readonly Dictionary<string, Func<View>> Routes = new Dictionary<string, Func<View>>();
		public void RegisterRoutes(Dictionary<string, Func<View>> routes) => routes.ToList().ForEach(pair => Routes[pair.Key] = pair.Value);

		protected virtual void OnInit()
		{

		}

		protected virtual void OnPause()
		{

		}

		protected virtual void OnResume()
		{

		}

		public virtual void Navigate(Uri uri)
		{

		}


		#region IDisposable Support
		void IApplicationLifeCycle.SetCurrentApp(CometApp app) => CurrentApp = app;

		void IApplicationLifeCycle.OnInit() => OnInit();

		void IApplicationLifeCycle.OnPause() => OnPause();

		void IApplicationLifeCycle.OnResume() => OnResume();

		void IApplicationLifeCycle.OnNavigate(Uri uri) => Navigate(uri);
		#endregion
	}
}
