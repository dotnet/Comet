using System;
namespace HotUI.Samples
{
    public class TabViewSample : View
    {
        public TabViewSample()
        {
            this.Title = "TabView sample";
        }

        [Body]
        View body() => new TabView
        {
            new HStack{
                new Text("Tab 1")
            }.TabText("Tab 1"),
            new HStack
            {
                new Text("Tab 2"),
            }.TabText("Tab 2")
        };
    }
}
