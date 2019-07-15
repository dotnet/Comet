using System;
using System.Collections.Generic;
using System.Text;

namespace HotUI.Samples
{
    public class ClipSample1 : View
    {
        [Body]
        View body() => new VStack {
                new Image("turtlerock.jpg")
                    .ClipShape(new Circle())
                    .Overlay(new Circle().Stroke(Color.White, lineWidth: 4))
                    .Shadow(radius: 10)
            };

    }
}
