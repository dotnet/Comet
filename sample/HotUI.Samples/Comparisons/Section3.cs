using System;
using System.Collections.Generic;
using System.Text;


/*
 
import SwiftUI

struct ContentView: View {
    var body: some View {
        VStack(alignment: .leading) {
            Text("Turtle Rock")
                .font(.title)
            HStack {
                Text("Joshua Tree National Park")
                    .font(.subheadline)
                Spacer()
                Text("California")
                    .font(.subheadline)
            }
        }
        .padding()
    }
}

 */

namespace HotUI.Samples.Comparisons
{
    public class Section3 : View
    {
        public Section3()
        {
            Body = () =>
                new VStack(alignment: HorizontalAlignment.Leading)
                {
                    new Text("Turtle Rock")
                        .SetFontSize(24),
                    new HStack()
                    {
                        new Text("Joshua Tree National Park")
                            .SetFontSize(12),
                        new Spacer(),
                        new Text("California")
                            .SetFontSize(12),
                    }
                }.Padding();
        }
    }
}
