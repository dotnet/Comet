using System.Drawing;

namespace Comet.Tests.Handlers
{
	public class SliderHandler : GenericViewHandler
	{
		public SliderHandler()
		{
			OnMeasure = HandleOnMeasure;
		}

		public Slider VirtualView => (Slider)CurrentView;

		private SizeF HandleOnMeasure(SizeF arg) => new SizeF(100, 20);
	}
}
