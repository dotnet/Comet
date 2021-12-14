using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.Maui.Graphics;

namespace Comet.Samples
{
	public class ClipSample1 : View
	{
		[Body]
		View body() => new VStack {
				new Image("turtlerock.jpg")
				.Aspect(Aspect.AspectFill)
					.ClipShape(new Circle())
					.Overlay(new Circle().Stroke(Colors.White, lineWidth: 4))
					.Shadow(radius: 10)
			};

	}
}
