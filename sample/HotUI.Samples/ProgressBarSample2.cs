using System.Threading;

namespace HotUI.Samples
{
    public class ProgressBarSample2 : View
    {
        public ProgressBarSample2()
        {
        }

        [Body]
        View body() => new ProgressBar(isIndeterminate: true);
    }
}