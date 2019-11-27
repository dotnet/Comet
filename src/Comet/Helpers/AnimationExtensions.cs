using System;

namespace Comet
{
	public static class AnimationExtensions
	{
		/// <summary>
		/// Apply animation to a given view
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view"></param>
		/// <param name="animation"></param>
		/// <returns></returns>
		public static T Animate<T>(this T view, Animation animation) where T : View
		{
			view.SetEnvironment(EnvironmentKeys.Animations.Animation, animation);
			return view;
		}

		public static Animation GetAnimation(this View view)
		{
			var animation = view.GetEnvironment<Animation>(EnvironmentKeys.Animations.Animation);
			return animation;
		}
	}
}
