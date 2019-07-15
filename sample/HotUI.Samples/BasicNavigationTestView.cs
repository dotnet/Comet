using System;
using System.Collections.Generic;

namespace HotUI.Samples
{
    public class BasicNavigationTestView : View
    {
        [Body]
        View body() => new NavigationView
        {
            new VStack()
            {
                new NavigationButton("Navigate!", () => new BasicTestView()),
            }
        };
    }



}