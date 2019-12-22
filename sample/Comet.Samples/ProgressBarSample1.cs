using System.Threading;

namespace Comet.Samples
{
    public class ProgressBarSample1 : View
    {
        readonly State<double> percentage = new State<double>(10);
        private readonly Timer _timer;

        public ProgressBarSample1()
        {
            _timer = new Timer(async state =>             {
                var p = (State<double>)state;
                await ThreadHelper.SwitchToMainThreadAsync();

                var current = p.Value;
                var value = current < 101 ? current + 1 : 0;

                p.Value = value;
            }, percentage, 100, 100);
        }

        [Body]
        View body() => new VStack()
        {
            new ProgressBar(percentage),
            new Text(()=>$"{percentage.Value} %"),
        };

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            // TODO: Stop when lifecycle events for views are available
            _timer.Dispose();
        }
    }
}
