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
			[nameof(EnvironmentKeys.View.Overlay)] = MapOverlayProperty,
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
			// todo: Console.WriteLine("Shadows not yet supported on Android");
		}

		public static void MapClipShapeProperty(IViewHandler handler, View virtualView)
		{
			var clipShape = virtualView.GetClipShape();
			if(clipShape != null)
			{
				handler.HasContainer = true;
				var viewHandler = handler as AndroidViewHandler;
				if(viewHandler?.ContainerView != null)
				{
					viewHandler.ContainerView.ClipShape = clipShape;
				}
			}
		}

		public static void MapOverlayProperty(IViewHandler handler, View virtualView)
		{
			
			var overlay = virtualView.GetOverlay();

			var viewHandler = handler as AndroidViewHandler;

			// If there is a clip shape, then the shadow should be applied to the clip layer, not the view layer
			if (overlay != null)
			{
				handler.HasContainer = true;
				if (viewHandler?.ContainerView != null)
				{
					viewHandler.ContainerView.View = virtualView;
					viewHandler.ContainerView.OverlayShapeView = new Skia.SkiaShapeView(overlay);
					
				}
			}

			
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
	}
}
