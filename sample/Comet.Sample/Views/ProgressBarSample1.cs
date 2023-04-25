using System.Threading;
using Comet.Styles;
namespace Comet.Samples
{
	public class DemoProgressBarStyle : ProgressBarStyle 
	{
		public DemoProgressBarStyle()
		{
			ProgressColor = Colors.Green;
		}
	}

	public class ProgressBarSample1 : View
	{
		readonly State<double> percentage = new State<double>(.1);
		private readonly Timer _timer;

		public ProgressBarSample1()
		{
			_timer = new Timer(state => {
				var p = (State<double>)state;
				var current = p.Value;
				var value = current < 1 ? current + .001f : 0;
				p.Value = value;
			}, percentage, 100, 100);
		}

		[Body]
		View body() => new VStack()
		{
			new ProgressBar(percentage).Apply<DemoProgressBarStyle>(),
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
