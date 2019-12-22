using System;
using CoreAnimation;
using CoreVideo;
using Foundation;

namespace Comet.Mac
{
    public class MacTicker : Ticker
    {
        public MacTicker()
        {
        }
        CVDisplayLink link;

        public override bool IsRunning => link != null;
        public override void Start()
        {
            if (link != null)
                return;
            link = new CVDisplayLink();
            link.SetOutputCallback(DisplayLinkOutputCallback);
            link.Start();

        }
        public CVReturn DisplayLinkOutputCallback(CVDisplayLink displayLink, ref CVTimeStamp inNow,
            ref CVTimeStamp inOutputTime, CVOptionFlags flagsIn, ref CVOptionFlags flagsOut)
        {
            Fire?.Invoke();
            return CVReturn.Success;
        }
        public override void Stop()
        {
            if (link == null)
                return;
            link?.Stop();
            link?.Dispose();
            link = null;
        }
    }
}
