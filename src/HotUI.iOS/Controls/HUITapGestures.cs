using System;
using UIKit;
namespace HotUI.iOS
{
    public class HUITapGesture : UITapGestureRecognizer
    {
        public HUITapGesture(TapGesture gesture) : base (()=> gesture.Action(gesture))
        {
            gesture.NativeGesture = this;
        }
        //TODO SetGesture Tate
        public override UIGestureRecognizerState State { get => base.State; set => base.State = value; }
       
    }
}
