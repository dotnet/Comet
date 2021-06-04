using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace CometApp1.Pages
{
	public class MainPage : View
	{
		public RideSample()
		{
			comet = new Comet();
		}

		[State]
		readonly Comet comet;

		[Body]
		View body()
			=> new VStack {
				new Text(()=> $"({comet.Rides}) rides taken:{comet.CometTrain}")
					.Frame(width:300)
					.LineBreakMode(LineBreakMode.CharacterWrap),

				new Button("Ride the Comet! ☄️", ()=>{
					comet.Rides++;
				})
					.Frame(height:44)
					.Margin(8)
					.Color(Colors.White)
					.Background(Colors.Green)
				.RoundedBorder(color:Colors.Blue)
				.Shadow(Colors.Grey,4,2,2),
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

