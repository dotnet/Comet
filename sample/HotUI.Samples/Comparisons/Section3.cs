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
                        .FontSize(24),
                    new HStack()
                    {
                        new Text("Joshua Tree National Park")
                            .FontSize(12),
                        new Spacer(),
                        new Text("California")
                            .FontSize(12),
                    }
                }.Padding();
        }
    }
}
