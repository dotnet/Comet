namespace CometApp1;
public class MainPage : View
{

	[State]
	public readonly CometRide comet = new();

	[Body]
	View body()
		=> new VStack {
				new Text(()=> $"({comet.Rides}) rides taken:{comet.CometTrain}")
					.Frame(width:300)
					.LineBreakMode(LineBreakMode.CharacterWrap),

				new Button("Ride the Comet! ☄️", ()=>{
					comet.Rides++;
				}).Tag("button")
					.Frame(height:44)
					.Margin(8)
					.Color(Colors.White)
					.Background(Colors.Green)
				.RoundedBorder(color:Colors.Blue)
				.Shadow(Colors.Grey,4,2,2),
		};

	public class CometRide : BindingObject
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


