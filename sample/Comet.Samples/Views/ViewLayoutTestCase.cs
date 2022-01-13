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
		View view() => //new ScrollView
					   //{
			new VStack{
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
			}.Background(Colors.LightGray);
		//};
	}
}
