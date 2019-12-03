using System;
using System.Timers;

namespace Comet
{
    public class Ticker
    {
		public Ticker()
        {
            timer.Elapsed += Timer_Elapsed;
        }

        void Timer_Elapsed(object sender, ElapsedEventArgs e) => Fire?.Invoke();

        Timer timer = new Timer
        {
            AutoReset = true,
			//60 fps
            Interval = 1000 / 60,
			
        };

        public virtual int MaxFps { get; set; } = 60;
		public Action Fire { get; set; }
        public virtual bool IsRunning => timer.Enabled;

        public virtual void Start()
        {
            timer.AutoReset = true;
            timer.Start();
        }
        public virtual void Stop()
        {
            timer.AutoReset = false;
            timer.Stop();
        }
    }
}
