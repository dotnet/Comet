using System;
using System.Collections.Generic;
using System.Text;

namespace System.Maui.Samples
{
	public class TextStylesSample : View
	{
		[Body]
		View body() => new VStack
		{
			new Label("H1").StyleAsH1(),
			new Label("H2").StyleAsH2(),
			new Label("H3").StyleAsH3(),
			new Label("H4").StyleAsH4(),
			new Label("H5").StyleAsH5(),
			new Label("H6").StyleAsH6(),
			new Label("Subtitle 1").StyleAsSubtitle1(),
			new Label("Subtitle 2").StyleAsSubtitle2(),
			new Label("Body 1").StyleAsBody1(),
			new Label("Body 2").StyleAsBody2(),
			new Label("Caption").StyleAsBody2(),
			new Label("OVERLINE").StyleAsOverline(),
		};
	}
}
