using Microsoft.Maui.Graphics;

namespace Comet.Tests.Handlers
{
	public class ProgressBarHandler : GenericViewHandler
	{
		public ProgressBarHandler()
		{
			OnGetIntrinsicSize = HandleOnGetIntrinsicSize;
		}

		public Slider VirtualView => (Slider)CurrentView;

		private Size HandleOnGetIntrinsicSize(Size arg) => new SizeF(100, 20);
	}
}
