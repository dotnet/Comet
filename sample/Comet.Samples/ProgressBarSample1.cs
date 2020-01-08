using System.Threading;

namespace Comet.Samples
{
	public class ProgressBarSample1 : View
	{
		readonly State<float> percentage = new State<float>(.1f);
		private readonly Timer _timer;

		public ProgressBarSample1()
		{
			_timer = new Timer(async state => {
				var p = (State<float>)state;
				await ThreadHelper.SwitchToMainThreadAsync();

				var current = p.Value;
				var value = current < 1 ? current + .001f : 0;

				p.Value = value;
			}, percentage, 100, 100);
		}

		[Body]
		View body() => new VStack()
		{
			new ProgressBar(percentage),
			new Text(()=>$"{percentage.Value.ToString("P2")}"),
		};

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			// TODO: Stop when lifecycle events for views are available
			_timer.Dispose();
		}
	}
}
