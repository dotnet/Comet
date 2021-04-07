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

		private Size HandleOnGetIntrinsicSize(Size arg) => new Size(100, 20);
	}
}
