using System;
using System.Collections.Generic;
using Comet.Styles;
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
	public class DemoSlideStyle : SliderStyle
	{
		public DemoSlideStyle()
		{
			TrackColor = Colors.Blue;
			ProgressColor = Colors.Green;
			ThumbColor = Colors.Red;
		}
	}

	public class SliderSample1 : View
	{
		readonly State<double> celsius = 50;

		[Body]
		View body() => new VStack
			{
                //new Slider(value: 12, from: -100, through: 100, by: 0.1f),
                //new Slider(value: () => 12f, from: -100, through: 100, by: 0.1f),
                //new Slider(value: new Binding<float>( getValue: () => 12f, setValue:null), from: -100, through: 100),
                new Slider(value: celsius, minimum: -100, maximum: 100).Apply<DemoSlideStyle>(),
				new Text(()=>$"{celsius.Value} Celsius"),
				new Text(()=>$"{celsius.Value * 9 / 5 + 32} Fahrenheit"),
			};

	}
}
