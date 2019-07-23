using System;
using System.Collections.Generic;
using System.Linq;
using Android.Views;
using AView = Android.Views.View;
namespace HotUI.Android.Controls
{
    public class HUITouchGestureListener : Java.Lang.Object, AView.IOnTouchListener
    {
        class GestureDetectorListener : Java.Lang.Object, GestureDetector.IOnGestureListener
        {
            GestureDetector gestureDetector;
            public GestureDetectorListener()
            {
                gestureDetector = new GestureDetector(AndroidContext.CurrentContext, this);
            }
           

            public bool OnDown(MotionEvent e)
            {
                return true;
            }

            public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
            {
                return true;
            }

            public void OnLongPress(MotionEvent e)
            {
            }

            public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
            {

                return true;
            }

            public void OnShowPress(MotionEvent e)
            {
            }

            public bool OnSingleTapUp(MotionEvent e)
            {
                dictionary[e].OnTap();
                return true;
            }
            Dictionary<MotionEvent, HUITouchGestureListener> dictionary = new Dictionary<MotionEvent, HUITouchGestureListener>();
            public bool OnTouchEvent(HUITouchGestureListener v, MotionEvent e)
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
                    if(isComplete)
                        dictionary.Remove(e);
                }
            }
        }

        AView view;
        GestureDetectorListener _gestureDetector;
        GestureDetectorListener gestureDetector => _gestureDetector ?? (_gestureDetector = new GestureDetectorListener());
        public HUITouchGestureListener(AView view)
        {
            this.view = view;
            view.SetOnTouchListener(this);
        }
        List<Gesture> gestures = new List<Gesture>();
        public void AddGesture(Gesture gesture)
        {
            gestures.Add(gesture);
        }

        public void RemoveGesture(Gesture gesture)
        {

            gestures.Remove(gesture);
        }

        protected void OnTap()
        {
            foreach (var g in gestures.OfType<TapGesture>())
                g.Invoke();
        }

        public bool OnTouch(AView v, MotionEvent e)

        {
            Console.WriteLine($"Touching view:{v}");
            return gestureDetector.OnTouchEvent(this,e);
        }
        protected override void Dispose(bool disposing)
        {
            view.SetOnTouchListener(null);
            base.Dispose(disposing);
        }

    }
}
