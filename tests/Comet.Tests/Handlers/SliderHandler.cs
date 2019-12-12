using System.Drawing;

namespace Comet.Tests.Handlers
{
	public class SliderHandler : GenericViewHandler
	{
		public SliderHandler()
		{
			OnGetIntrinsicSize = HandleOnGetIntrinsicSize;
		}

		public Slider VirtualView => (Slider)CurrentView;

		private SizeF HandleOnGetIntrinsicSize(SizeF arg) => new SizeF(100, 20);
	}
}
