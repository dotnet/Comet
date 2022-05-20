using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comet.Samples
{
	public  class ViewLayoutTestCase : View
	{
		[Body]
		View view() =>  new ScrollView {
			new VStack(){

				new VStack
				{
					new Text(()=> "Recommended")
						.Color(Colors.Black)
						.FontFamily("Rockolf Bold")
						.FontSize(20)
						.FontWeight(FontWeight.Bold)
						.Margin(new Thickness(0, 6)),
					new ScrollView(Orientation.Horizontal) {
						new HStack
						{
							Enumerable.Range(0,10).Select(destination => new ZStack
						{
							// Destination Background Image
							new Image()
								.Background(Colors.SkyBlue).FillHorizontal().FillVertical(),
							new VStack(LayoutAlignment.Start) {
								new VStack
								{
									new Text(() => "$100")
										.Color(Colors.White)
										.FitHorizontal()
										.FontSize(14)
										.FontFamily("Rockolf Bold")
										.FontSize(14)
										.FontWeight(FontWeight.Bold),
								}.FitHorizontal().Alignment( Alignment.Trailing)
									.Background(Color.FromArgb("#67AEE9"))
									.ClipShape(new RoundedRectangle(12))
									.Padding(6)
									.Margin(12),

								new Spacer(),
								new Text("Japan Street")
									.Color(Colors.White)
									.FontFamily("Rockolf Bold")
									.FontSize(18)
									.FontWeight(FontWeight.Bold)
									.Shadow(radius: 6),
								new Text("Awesome Sauce")
									.Color(Colors.White)
									.FontFamily("Rockolf")
									.FontSize(14),
							}
							.Padding(new Thickness(16, 0, 0, 16))
						}.ClipShape(new RoundedRectangle(36)).Frame(height: 250, width: 200))
						}
					}
				},


				new Text("ZSTack Alignment"),
				new ZStack
				{
					new Text("TL").Background(Colors.Blue).Frame(75,75).Alignment(Alignment.TopLeading),
					new Text("T").Background(Colors.Blue).Frame(75,75).Alignment( Alignment.Top),
					new Text("TR").Background(Colors.Blue).Frame(75,75).Alignment( Alignment.TopTrailing),
					new Text("R").Background(Colors.Blue).Frame(75,75).Alignment( Alignment.Trailing),
					new Text("L").Background(Colors.Blue).Frame(75,75).Alignment( Alignment.Leading),
					new Text("BL").Background(Colors.Blue).Frame(75,75).Alignment( Alignment.BottomLeading),
					new Text("BR").Background(Colors.Blue).Frame(75,75).Alignment( Alignment.BottomTrailing),
					new Text("B").Background(Colors.Blue).Frame(75,75).Alignment( Alignment.Bottom),
				}.Frame(400,400).Padding(12)
				.Background(Colors.White),

				new Text("HStack, Only uses Vertial Alignment"),
				new HStack
				{
					new Text("T").Background(Colors.Blue).Frame(75,75).Alignment( Alignment.Top),
					new Text("Center").Background(Colors.Blue).Frame(75,75).Alignment(Alignment.Center),
					new Text("B").Background(Colors.Blue).Frame(75,75).Alignment( Alignment.Bottom),

				}.Frame(400,400).Background(Colors.White).Padding(12),

				new Text("VStack, Only uses Horizontal Alignment"),
				new VStack
				{
					new Text("L").Background(Colors.Blue).Frame(75,75).Alignment( Alignment.Leading),
					new Text("C").Background(Colors.Blue).Frame(75,75).Alignment( Alignment.Top),
					new Text("R").Background(Colors.Blue).Frame(75,75).Alignment( Alignment.Trailing),

				}.Frame(400,400).Background(Colors.White).Padding(12),

				new Text("VStack Without Spacers"),
				new VStack
				{
					new Text("L").Background(Colors.Blue),
					new Text("C").Background(Colors.Blue),
					new Text("R").Background(Colors.Blue),

				}.Frame(400,400).Background(Colors.White).Padding(12),

				new Text("VStack With Spacers"),
				new VStack
				{
					new Text("L").Background(Colors.Blue),
					new Spacer(),
					new Text("C").Background(Colors.Blue),
					new Text("R").Background(Colors.Blue),

				}.Frame(200,200).Background(Colors.White).Padding(12),

				new Text("HStack Without Spacers"),
				new HStack
				{
					new Text("L").Background(Colors.Blue),
					new Text("C").Background(Colors.Blue),
					new Text("R").Background(Colors.Blue),

				}.Background(Colors.White).Padding(12),

				new Text("HStack With Spacers"),
				new HStack
				{
					new Text("L").Background(Colors.Blue),
					new Spacer(),
					new Text("C").Background(Colors.Blue),
					new Text("R").Background(Colors.Blue),

				}.Frame(200,200).Background(Colors.White).Padding(12),

			}
		};
	}
}
