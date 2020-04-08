using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
// ReSharper disable MemberCanBePrivate.Global

namespace System.Maui.UWP.Handlers
{
	public class ViewHandler : AbstractHandler<View, UIElement>
	{
		public static readonly PropertyMapper<View> Mapper = new PropertyMapper<View>()
		{
			[nameof(EnvironmentKeys.Colors.BackgroundColor)] = MapBackgroundColorProperty,
			[nameof(EnvironmentKeys.View.Shadow)] = MapShadowProperty,
			[nameof(EnvironmentKeys.View.ClipShape)] = MapClipShapeProperty,
			[nameof(EnvironmentKeys.View.Overlay)] = MapOverlayProperty,
		};

		protected override UIElement CreateView()
		{
			var viewHandler = VirtualView?.GetOrCreateViewHandler();
			if (viewHandler?.GetType() == typeof(ViewHandler) && VirtualView.Body == null)
			{
				Debug.WriteLine($"There is no ViewHandler for {VirtualView.GetType()}");
				return null;
			}

			return viewHandler?.View;
		}

		private static void MapOverlayProperty(IViewHandler arg1, View arg2)
		{
		}

		private static void MapClipShapeProperty(IViewHandler arg1, View arg2)
		{
		}

		private static void MapShadowProperty(IViewHandler arg1, View arg2)
		{
		}

		private static void MapBackgroundColorProperty(IViewHandler arg1, View arg2)
		{
		}
	}
}
