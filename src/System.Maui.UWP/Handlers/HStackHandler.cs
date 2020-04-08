using Windows.UI.Xaml.Controls;
// ReSharper disable ClassNeverInstantiated.Global
using UWPOrientation = Windows.UI.Xaml.Controls.Orientation;

namespace System.Maui.UWP.Handlers
{
	public class HStackHandler : AbstractStackLayoutHandler
	{

		public HStackHandler()
		{
			Orientation = UWPOrientation.Horizontal;
		}
	}
}
