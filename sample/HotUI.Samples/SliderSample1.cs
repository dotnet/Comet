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
namespace HotUI.Samples
{
    public class SliderSample1 : View
    {
        readonly State<float> celsius = 50;
        
        public SliderSample1()
        {
            Body = () => new VStack    
            {
                new Slider(value: celsius, from: -100, through: 100, by: 0.1f),
                new Text($"{celsius.Value} Celsius"),
                new Text($"{celsius.Value * 9 / 5 + 32} Fahrenheit"),
            };
        }
    }
}