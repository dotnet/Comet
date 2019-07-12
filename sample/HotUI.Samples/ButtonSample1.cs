using System;
using System.Collections.Generic;

namespace HotUI.Samples
{
    public class ButtonSample1 : View
    {
        readonly State<int> count = 0;
        
        public ButtonSample1()
        {
            Body = () => new VStack
            {
                new Button("Increment Value", () => count.Value = count + 1),
                new Text($"Value: {count.Value}"),
            };
        }
    }
}