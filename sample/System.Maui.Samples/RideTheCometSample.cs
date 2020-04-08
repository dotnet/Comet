using System;
using System.Collections.Generic;
using System.Text;
using System.Maui.Skia;

/*
 
import SwiftUI

struct ContentView: View {
    var body: some View {
        Text("Hello SwiftUI!")
    }
}

 */

namespace System.Maui.Samples.Comparisons
{
	public class RideSample : View
	{
		public RideSample()
		{
			//View.SetGlobalEnvironment(EnvironmentKeys.Colors.Color, Color.Black);
			Maui = new MauiClass();
		}

		[State]
		readonly MauiClass Maui;

		[Body]
		View body()
			=> new VStack {
				new Text(()=> $"({Maui.Rides}) rides taken:{Maui.MauiTrain}")
					.Frame(width:300)
					.LineBreakMode(LineBreakMode.CharacterWrap)
					,

				new Button("Ride the System.Maui! ☄️", ()=>{
					Maui.Rides++;
				})
					.Frame(height:44)
					.Margin(8)
					.Color(Color.White)
					.Background(Color.Green)
				.RoundedBorder(color:Color.Blue)
				.Shadow(Color.Grey,4,2,2),
			};

		public class MauiClass : BindingObject
		{
			public int Rides
			{
				get => GetProperty<int>();
				set => SetProperty(value);
			}

			public string MauiTrain
			{
				get
				{
					return "☄️".Repeat(Rides);
				}
			}
		}
	}
}

