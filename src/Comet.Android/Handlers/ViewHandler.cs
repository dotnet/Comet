using System;
using System.Linq;
using Android.Content;
using AView = Android.Views.View;

namespace Comet.Android.Handlers
{
	public class ViewHandler : AbstractHandler<View, AView>
	{
		public static readonly PropertyMapper<View> Mapper = new PropertyMapper<View>()
		{
			[nameof(EnvironmentKeys.Colors.BackgroundColor)] = MapBackgroundColorProperty,
			[nameof(EnvironmentKeys.View.Shadow)] = MapShadowProperty,
			[nameof(EnvironmentKeys.View.ClipShape)] = MapClipShapeProperty,
			[nameof(EnvironmentKeys.Animations.Animation)] = MapAnimationProperty,
		};

		public ViewHandler() : base(Mapper)
		{
		}

		protected override AView CreateView(Context context)
		{
			return VirtualView.ToView();
		}

		public static void MapBackgroundColorProperty(IViewHandler handler, View virtualView)
		{
			var nativeView = (AView)handler.NativeView;
			var color = virtualView.GetBackgroundColor();
			if (color != null)
				nativeView.SetBackgroundColor(color.ToColor());
		}

		public static void MapShadowProperty(IViewHandler handler, View virtualView)
		{
			Console.WriteLine("Shadows not yet supported on Android");
		}

		public static void MapClipShapeProperty(IViewHandler handler, View virtualView)
		{
			Console.WriteLine("ClipShape not yet supported on Android");
		}

		public static void AddGestures(AndroidViewHandler handler, View view)
		{
			var gestures = view.Gestures;
			if (!(gestures?.Any() ?? false))
				return;
			var listner = handler.GetGestureListener();
			foreach (var gesture in gestures)
				listner.AddGesture(gesture);
		}

		public static void AddGesture(AndroidViewHandler handler, Gesture gesture)
		{
			var listner = handler.GetGestureListener();
			listner.AddGesture(gesture);

		}

		public static void RemoveGestures(AndroidViewHandler handler, View view)
		{
			var gestures = view.Gestures;
			if (!(gestures?.Any() ?? false))
				return;
			var listner = handler.GetGestureListener();
			foreach (var gesture in gestures)
				listner.RemoveGesture(gesture);
			listner.Dispose();

		}

		public static void RemoveGesture(AndroidViewHandler handler, Gesture gesture)
		{
			var listner = handler.GetGestureListener();
			listner.RemoveGesture(gesture);
		}

		public static void MapAnimationProperty(IViewHandler handler, View virtualView)
		{
			var nativeView = (AView)handler.NativeView;
			var animation = virtualView.GetAnimation();
			if (animation != null)
			{
				System.Diagnostics.Debug.WriteLine($"Starting animation [{animation}] on [{virtualView.GetType().Name}/{nativeView.GetType().Name}]");

				var duration = Convert.ToInt64(animation.Duration ?? 1000);
				var delay = Convert.ToInt64(animation.Delay ?? 0);
				var animator = nativeView.Animate();
				animator.SetStartDelay(delay);
				animator.SetDuration(duration);

				if (animation.TranslateTo != null)
				{
					animator.TranslationX(animation.TranslateTo.Value.X);
					animator.TranslationY(animation.TranslateTo.Value.Y);
				}

				if (animation.RotateTo != null)
				{
					var angle = Convert.ToInt16(animation.RotateTo.Value);
					animator.Rotation(angle);
				}

				if (animation.ScaleTo != null)
				{
					animator.ScaleX(animation.ScaleTo.Value.X);
					animator.ScaleY(animation.ScaleTo.Value.Y);
				}

				animator.Start();

				// TODO: implement other properties
			}
		}
	}
}
