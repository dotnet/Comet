using System.Threading;

namespace Comet.Samples
{
	public class ProgressBarSample2 : View
	{
		public ProgressBarSample2()
		{
		}

		[Body]
		View body() => new ProgressBar();
	}
}
