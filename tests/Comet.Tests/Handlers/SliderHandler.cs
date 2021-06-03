using Microsoft.Maui.Graphics;

namespace Comet.Tests.Handlers
{
	public class SliderHandler : GenericViewHandler
	{
		public SliderHandler()
		{
			OnGetIntrinsicSize = HandleOnGetIntrinsicSize;
		}

		public Slider VirtualView => (Slider)CurrentView;

		private Size HandleOnGetIntrinsicSize(double widthConstraint, double heightConstraint) => new Size(100, 20);
	}
}
