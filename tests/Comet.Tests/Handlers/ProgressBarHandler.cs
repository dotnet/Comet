using System.Drawing;

namespace Comet.Tests.Handlers
{
	public class ProgressBarHandler : GenericViewHandler
	{
		public ProgressBarHandler()
		{
			OnMeasure = HandleOnMeasure;
		}

		public Slider VirtualView => (Slider)CurrentView;

		private SizeF HandleOnMeasure(SizeF arg) => new SizeF(100, 20);
	}
}
