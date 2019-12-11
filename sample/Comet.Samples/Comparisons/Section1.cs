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

namespace Comet.Samples.Comparisons
{
	public class Section1 : View
	{
		[Body]
		View Body()
			=> new Text("Hello Comet!");
	}
}
