using System;
using System.Collections.Generic;
using System.Text;


/*
 
import SwiftUI

struct CircleImage: View {
    var body: some View {
        Image("turtlerock")
            .clipShape(Circle())
            .overlay(
                Circle().stroke(Color.white, lineWidth: 4))
            .shadow(radius: 10)
    }
}

 */

namespace Comet.Samples.Comparisons
{
	public class Section4c : View
	{
		[Body]
		View body() => new VStack {
				new Image("turtlerock.jpg")
					.Shadow(radius: 10)
			};

	}
}
