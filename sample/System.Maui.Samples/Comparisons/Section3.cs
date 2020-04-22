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

namespace System.Maui.Samples.Comparisons
{
	public class Section3 : View
	{
		[Body]
		View body() =>
				 new VStack(alignment: HorizontalAlignment.Leading){
					new Label("Turtle Rock"),
					new HStack {
						new Label("Joshua Tree National Park")
							.Background(Color.Salmon),
						new Spacer(),
						new Label("California")
							.Background(Color.Green),
					}
				 }.Margin();

	}
}
