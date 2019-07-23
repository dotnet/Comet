using System;
namespace HotUI
{
    public class TapGesture : Gesture<TapGesture>
    {
        public TapGesture(Action<TapGesture> action) : base(action)
        {

        }
    }
}
