using System;
using System.Collections.Generic;
using System.Text;


/*
 
import SwiftUI

struct ContentView: View {
    var body: some View {
        Text("Hello SwiftUI!")
    }
}

 */

namespace HotUI.Samples.Comparisons
{
    public class Section1 : View
    {
        public Section1()
        {
            Body = () => 
                new Text("Hello HotUI!");
        }
    }
}
