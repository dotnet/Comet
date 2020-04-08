using System;
using System.Diagnostics;
using AppKit;
using CoreAnimation;
using CoreGraphics;
using System.Maui.Mac.Extensions;

namespace System.Maui.Mac.Handlers
{
	public class ViewHandler : AbstractHandler<View, NSView>
	{
		public static readonly PropertyMapper<View> Mapper = new PropertyMapper<View>()
		{
			[nameof(EnvironmentKeys.Colors.BackgroundColor)] = MapBackgroundColorProperty,
			[nameof(EnvironmentKeys.View.Shadow)] = MapShadowProperty,
			[nameof(EnvironmentKeys.View.ClipShape)] = MapClipShapeProperty,
			[nameof(EnvironmentKeys.View.Border)] = MapBorderProperty
		};

		protected override NSView CreateView()
		{
			var viewHandler = VirtualView?.GetOrCreateViewHandler();
			if (viewHandler?.GetType() == typeof(ViewHandler) && NativeView == null)
			{
				// this is recursive.
				Debug.WriteLine($"There is no ViewHandler for {VirtualView.GetType()}");
				return null;
			}

			return viewHandler?.View ?? new NSColorView();
		}

		public override void SetView(View view)
		{
			var previousView = TypedNativeView;
			base.SetView(view);
			BroadcastNativeViewChanged(previousView, TypedNativeView);
		}

		public static void MapBackgroundColorProperty(IViewHandler viewHandler, View virtualView)
		{
			var nativeView = (NSView)viewHandler.NativeView;
			var color = virtualView.GetBackgroundColor();
			if (color != null)
			{
				if (nativeView is NSColorView colorView)
				{
					colorView.BackgroundColor = color.ToNSColor();
				}
				else if (nativeView is NSTextField textField)
				{
					textField.BackgroundColor = color.ToNSColor();
					textField.DrawsBackground = true;
				}
			}
		}

		public static void MapShadowProperty(IViewHandler handler, View virtualView)
		{
			var nativeView = (NSView)handler.NativeView;
			var shadow = virtualView.GetShadow();
			var clipShape = virtualView.GetClipShape();

			// If there is a clip shape, then the shadow should be applied to the clip layer, not the view layer
			if (shadow != null && clipShape == null)
			{
				handler.HasContainer = false;
				nativeView.Shadow = new NSShadow()
				{
					ShadowColor = shadow.Color.ToNSColor(),
					ShadowOffset = shadow.Offset.ToCGSize(),
					ShadowBlurRadius = shadow.Radius
				};

				/*if (nativeView.Layer == null)
                    nativeView.WantsLayer = true;
                
                ApplyShadowToLayer(shadow, nativeView.Layer);*/
			}
			else if (nativeView != null)
			{
				// todo: Xamarin.Mac bug, you should be able to set Shadow to null.  Either get them to fix this,
				// or use the Objective-C runtime to set this.
				nativeView.Shadow = new NSShadow()
				{
					ShadowBlurRadius = 0,
					ShadowColor = NSColor.Clear,
					ShadowOffset = new CGSize()
				};
			}
		}

		public static void MapClipShapeProperty(IViewHandler handler, View virtualView)
		{
			var nativeView = (NSView)handler.NativeView;
			var clipShape = virtualView.GetClipShape();
			if (clipShape != null)
			{
				handler.HasContainer = true;
				var bounds = nativeView.Bounds;

				var layer = new CAShapeLayer
				{
					Frame = bounds
				};

				var path = clipShape.PathForBounds(bounds.ToRectangleF());
				layer.Path = path.ToCGPath();

				var viewHandler = handler as MacViewHandler;
				if (viewHandler?.ContainerView != null)
					viewHandler.ContainerView.MaskLayer = layer;

				var shadow = virtualView.GetShadow();
				if (shadow != null)
				{
					if (viewHandler.ContainerView != null)
					{
						viewHandler.ContainerView.Shadow = new NSShadow()
						{
							ShadowColor = shadow.Color.ToNSColor(),
							ShadowOffset = shadow.Offset.ToCGSize(),
							ShadowBlurRadius = shadow.Radius
						};
					}
				}
			}
			else
			{
				handler.HasContainer = false;
			}
		}

		public static void MapBorderProperty(IViewHandler viewHandler, View virtualView)
		{

		}
	}
}
