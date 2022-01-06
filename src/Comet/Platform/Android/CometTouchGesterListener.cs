using System;
using System.Collections.Generic;
using System.Linq;
using Android.Views;
using AView = Android.Views.View;
namespace Comet.Android.Controls
{
	public class CometTouchGestureListener : Java.Lang.Object, AView.IOnTouchListener
	{
		class GestureDetectorListener : Java.Lang.Object, GestureDetector.IOnGestureListener
		{
			readonly GestureDetector gestureDetector;
			public GestureDetectorListener(View view)
			{
				gestureDetector = new GestureDetector(view.GetMauiContext().Context, this);
			}


			public bool OnDown(MotionEvent e) => true;

			public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY) => true;

			public void OnLongPress(MotionEvent e)
			{
			}

			public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY) => true;

			public void OnShowPress(MotionEvent e)
			{
			}

			public bool OnSingleTapUp(MotionEvent e)
			{
				dictionary[e].OnTap();
				return true;
			}
			Dictionary<MotionEvent, CometTouchGestureListener> dictionary = new Dictionary<MotionEvent, CometTouchGestureListener>();
			public bool OnTouchEvent(CometTouchGestureListener v, MotionEvent e)
			{
				var isComplete = e.IsComplete();
				try
				{
					if (!isComplete)
						dictionary[e] = v;
					Logger.Debug($"Touch dictionary {dictionary.Count}");
					return gestureDetector.OnTouchEvent(e);
				}
				finally
				{
					if (isComplete)
						dictionary.Remove(e);
				}
			}
		}

		AView nativeView;
		View view;
		GestureDetectorListener _gestureDetector;
		GestureDetectorListener gestureDetector => _gestureDetector ?? (_gestureDetector = new GestureDetectorListener(view));
		public CometTouchGestureListener(AView nativeView, View view)
		{
			this.view = view;
			this.nativeView = nativeView;
			nativeView.SetOnTouchListener(this);
		}
		List<Gesture> gestures = new List<Gesture>();
		public void AddGesture(Gesture gesture) => gestures.Add(gesture);

		public void RemoveGesture(Gesture gesture) => gestures.Remove(gesture);

		protected void OnTap()
		{
			foreach (var g in gestures.OfType<TapGesture>())
				g.Invoke();
		}

		public bool OnTouch(AView v, MotionEvent e)
		{
			Console.WriteLine($"Touching view:{v}");
			return gestureDetector.OnTouchEvent(this, e);
		}
		protected override void Dispose(bool disposing)
		{
			nativeView.SetOnTouchListener(null);
			base.Dispose(disposing);
		}

	}
}
