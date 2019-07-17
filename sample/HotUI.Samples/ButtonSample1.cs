using System;
using System.Collections.Generic;

namespace HotUI.Samples
{
    public class ButtonSample1 : View
    {
        readonly State<int> count = 0;

        [Body]
        View body() => new VStack
        {
            new Button("Increment Value", () => count.Value = count + 1),
            new Text(() => $"Value: {count.Value}"),
        };

    }
}