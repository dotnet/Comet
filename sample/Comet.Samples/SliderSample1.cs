using System;
using System.Collections.Generic;

/*

struct ContentView : View {
    @State var celsius: Double = 0

    var body: some View {
        VStack {
            Slider(value: $celsius, from: -100, through: 100, by: 0.1)
            Text("\(celsius) Celsius is \(celsius * 9 / 5 + 32) Fahrenheit")
        }
    }
}

*/
namespace Comet.Samples
{
    public class SliderSample1 : View
    {
        readonly State<float> celsius = 50;

        [Body]
        View body() => new VStack
            {
                //new Slider(value: 12, from: -100, through: 100, by: 0.1f),
                //new Slider(value: () => 12f, from: -100, through: 100, by: 0.1f),
                //new Slider(value: new Binding<float>( getValue: () => 12f, setValue:null), from: -100, through: 100, by: 0.1f),
                new Slider(value: celsius, from: -100, through: 100, by: 0.1f),
                new Text(()=>$"{celsius.Value} Celsius"),
                new Text(()=>$"{celsius.Value * 9 / 5 + 32} Fahrenheit"),
            };

    }
}
