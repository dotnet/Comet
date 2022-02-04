using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.Maui.Graphics;


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
	public class Section4 : View
	{
		[Body]
		View body() =>
			new Image("turtlerock.jpg")
				.ClipShape(new Circle())
				.Border(new Circle().Stroke(Colors.White, lineWidth: 4))
				.Shadow(radius: 10).Background(Colors.Green);
	}

}
