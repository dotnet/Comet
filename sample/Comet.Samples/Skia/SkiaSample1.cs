using System;
using System.Collections.Generic;
using System.Text;

namespace Comet.Samples.Skia
{
    /// <summary>
    /// This example how to use use a DrawableControl directly: you give it a control delegate
    /// in it's constructor.
    /// </summary>
    public class SkiaSample1 : View
    {
        [Body]
        View body() => new DrawableControl(new SimpleFingerPaint());

    }
}
