using System;
using UIKit;

namespace HotUI.iOS
{
    public static class AnimationExtensions
    {
        public static UIViewAnimationOptions ToAnimationOptions(this AnimationOptions options)
        {
            var result = UIViewAnimationOptions.TransitionNone;
            if (options != AnimationOptions.None)
            {
                if (options.HasFlag(AnimationOptions.Repeat))
                    result = result | UIViewAnimationOptions.Repeat;
                if (options.HasFlag(AnimationOptions.CurveEaseIn))
                    result = result | UIViewAnimationOptions.CurveEaseIn;
                if (options.HasFlag(AnimationOptions.CurveEaseInOut))
                    result = result | UIViewAnimationOptions.CurveEaseInOut;
                if (options.HasFlag(AnimationOptions.CurveEaseOut))
                    result = result | UIViewAnimationOptions.CurveEaseOut;
            }

            return result;
        }

        public static UIViewAnimationOptions ToAnimationOptions(this AnimationOptions? options)
        {
            return (options ?? AnimationOptions.None).ToAnimationOptions();
        }
    }
}
