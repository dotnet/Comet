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
    public class RideSample : View
    {
        public RideSample()
        {
            //View.SetGlobalEnvironment(EnvironmentKeys.Colors.Color, Color.Black);
            comet = new Comet();
        }

        [State]
        readonly Comet comet;

        [Body]
        View body()
            => new VStack {
                new Text(()=> $"({comet.Rides}) rides taken: {comet.CometTrain}")
                    .Frame(width:300)
                    .LineBreakMode(LineBreakMode.CharacterWrap)
                    ,

                new Button("   Ride the Comet!☄️   ", ()=>{
                    comet.Rides++;
                })
                    .Frame(height:44)
                    .Margin(8)
                    .Color(Color.White)
                    .Background("#1d1d1d")
            };

        public class Comet : BindingObject
        {
            public int Rides
            {
                get => GetProperty<int>();
                set => SetProperty(value);
            }

            public string CometTrain
            {
                get
                {
                    return "☄️".Repeat(Rides);
                }
            }
        }
    }
}

