using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Comet.Internal;
using Microsoft.Maui;

namespace Comet
{
	public static class ViewExtensions
	{
		public static View GetViewWithTag(this View view, string tag)
		{
			if (view == null) return null;

			if (view.Tag == tag)
				return view;

			if (view is AbstractLayout layout)
			{
				foreach (var subView in layout)
				{
					var match = subView.GetViewWithTag(tag);
					if (match != null)
						return match;
				}
			}

			if (view.GetType() == typeof(ContentView))
				return ((ContentView)view).Content.GetViewWithTag(tag);

			return view.BuiltView.GetViewWithTag(tag);
		}

		public static T GetViewWithTag<T>(this View view, string tag) where T : View => view.GetViewWithTag(tag) as T;

		public static T Tag<T>(this T view, string tag) where T : View
		{
			view.Tag = tag;
			return view;
		}

		public static ListView<T> OnSelected<T>(this ListView<T> listview, Action<T> selected)
		{
			listview.ItemSelected = (o) => {
				selected?.Invoke((T)o);
			};
			return listview;
		}

		public static List<FieldInfo> GetFieldsWithAttribute(this object obj, Type attribute)
		{
			var type = obj.GetType();
			var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Where(x => Attribute.IsDefined(x, attribute)).ToList();
			return fields;
		}

		public static T Title<T>(this T view, Binding<string> title, bool cascades = true) where T : View =>
			view.SetEnvironment(EnvironmentKeys.View.Title, title, cascades, ControlState.Default);
		public static T Title<T>(this T view, Func<string> title, bool cascades = true) where T : View =>
			view.Title((Binding<string>)title, cascades);

		public static string GetTitle(this View view)
		{
			var title = view?.GetEnvironment<string>(EnvironmentKeys.View.Title);
			title ??= view?.BuiltView?.GetEnvironment<string>(EnvironmentKeys.View.Title,true) ?? "";
			return title;
		}

		//public static T AddGesture<T>(this T view, Gesture gesture) where T : View
		//{
		//	var gestures = (List<Gesture>)(view.Gestures ?? (view.Gestures = new List<Gesture>()));
		//	gestures.Add(gesture);
		//	view?.ViewHandler?.UpdateValue(Comet.Gesture.AddGestureProperty);
		//	return view;
		//}
		//public static T RemoveGesture<T>(this T view, Gesture gesture) where T : View
		//{
		//	var gestures = (List<Gesture>)view.Gestures;
		//	gestures.Remove(gesture);
		//	view?.ViewHandler?.UpdateValue(Comet.Gesture.RemoveGestureProperty);
		//	return view;
		//}

		//public static T OnTap<T>(this T view, Action<T> action) where T : View
		//	=> view.AddGesture(new TapGesture((g) => action?.Invoke(view)));

		//public static T OnTapNavigate<T>(this T view, Func<View> destination) where T : View
		//	=> view.OnTap((v) => NavigationView.Navigate(view, destination.Invoke()));

		public static void Navigate(this View view, View destination) => NavigationView.Navigate(view, destination);

		public static void Dismiss(this View view) => NavigationView.Pop(view);

		public static ListView<T> OnSelectedNavigate<T>(this ListView<T> view, Func<T, View> destination) => view.OnSelected(v => NavigationView.Navigate(view, destination?.Invoke(v)));

		public static void SetResult<T>(this View view, T value)
		{
			var resultView = view.FindParentOfType<ResultView<T>>();
			resultView.SetResult(value);
		}


		public static void SetResult<T>(this View view, State<T> value)
		{
			var resultView = view.FindParentOfType<ResultView<T>>();
			resultView.SetResult(value.Value);
		}

		public static void SetResultCanceled<T>(this View view)
		{
			var resultView = view.FindParentOfType<ResultView<T>>();
			resultView.Cancel();
		}
		public static void SetResultException<T>(this View view, Exception ex)
		{
			var resultView = view.FindParentOfType<ResultView<T>>();
			resultView.SetException(ex);
		}

		public static string GetAutomationId(this View view)
			=> view.GetEnvironment<string>(view, EnvironmentKeys.View.AutomationId,cascades:false);
		public static void SetAutomationId(this View view, string automationId)
			=> view.SetEnvironment(EnvironmentKeys.View.AutomationId, automationId, cascades: false);

		/// <summary>
		/// Hunts through the parents to find the current Context.
		/// </summary>
		/// <param name="view"></param>
		/// <returns></returns>
		public static IMauiContext GetMauiContext(this View view)
		{
			//IF there is only one app, with one window, then there is only one context.
			//Don't go hunting!
			if (CometApp.CurrentApp.Windows.Count == 1)
				return CometApp.MauiContext;
			return view.FindParentOfType<IMauiContextHolder>()?.MauiContext ?? CometApp.MauiContext;
		}

		public static T Aspect<T>(this T image, Aspect aspect) where T : Image =>
			image.SetEnvironment(nameof(Aspect), aspect);

	}
}
