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
		View view() => new ScrollView
					   {
			new VStack(){
				new Text("ZSTack Alignment"),
				new ZStack
				{
					new Text("TL").Background(Colors.Blue).Frame(75,75, Alignment.TopLeading),
					new Text("T").Background(Colors.Blue).Frame(75,75, Alignment.Top),
					new Text("TR").Background(Colors.Blue).Frame(75,75, Alignment.TopTrailing),
					new Text("R").Background(Colors.Blue).Frame(75,75, Alignment.Trailing),
					new Text("L").Background(Colors.Blue).Frame(75,75, Alignment.Leading),
					new Text("BL").Background(Colors.Blue).Frame(75,75, Alignment.BottomLeading),
					new Text("BR").Background(Colors.Blue).Frame(75,75, Alignment.BottomTrailing),
					new Text("B").Background(Colors.Blue).Frame(75,75, Alignment.Bottom),
				}.Frame(400,400).Padding(12)
				.Background(Colors.White),

				new Text("HStack, Only uses Vertial Alignment"),
				new HStack
				{
					new Text("T").Background(Colors.Blue).Frame(75,75, Alignment.Top),
					new Text("Center").Background(Colors.Blue).Frame(75,75),
					new Text("B").Background(Colors.Blue).Frame(75,75, Alignment.Bottom),

				}.Frame(400,400).Background(Colors.White).Padding(12),

				new Text("VStack, Only uses Horizontal Alignment"),
				new VStack
				{
					new Text("L").Background(Colors.Blue).Frame(75,75, Alignment.Leading),
					new Text("C").Background(Colors.Blue).Frame(75,75, Alignment.Top),
					new Text("R").Background(Colors.Blue).Frame(75,75, Alignment.Trailing),

				}.Frame(400,400).Background(Colors.White).Padding(12),

				new Text("Layout Without Spacers"),
				new VStack
				{
					new Text("L").Background(Colors.Blue),
					new Text("C").Background(Colors.Blue),
					new Text("R").Background(Colors.Blue),

				}.Background(Colors.White).Padding(12),

				new Text("Layout With Spacers"),
				new VStack
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
