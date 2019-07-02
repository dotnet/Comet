using System;
using System.Collections.Generic;

namespace HotUI.Samples
{
    public class BasicNavigationTestView : View
    {
        public BasicNavigationTestView()
        {
            Body = () => new NavigationView
            {
                new VStack()
                {
                    new NavigationButton("Navigate!", () => new BasicTestView()),
                }
            };
        }


    }
}