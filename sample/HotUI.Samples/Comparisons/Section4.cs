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

namespace HotUI.Samples.Comparisons
{
    public class Section4 : View
    {
        public Section4()
        {
            Body = () =>
                new Image("http://lh3.googleusercontent.com/9Ofo9ZHQODFvahjpq2ZVUUOog4v5J1c4Gw9qjTw-KADTQZ6sG98GA1732mZA165RBoyxfoMblA")
                    .ClipShape(new Circle())
                    .Overlay(
                        new Circle().Stroke(Color.White, lineWidth: 4));
        }
    }
}
