using System;
using System.Collections.Generic;
using System.Text;

namespace Comet.Samples
{
    public class TextStylesSample : View
    {
        [Body]
        View body() => new VStack
        {
            new Text("H1").StyleAsH1(),
            new Text("H2").StyleAsH2(),
            new Text("H3").StyleAsH3(),
            new Text("H4").StyleAsH4(),
            new Text("H5").StyleAsH5(),
            new Text("H6").StyleAsH6(),
            new Text("Subtitle 1").StyleAsSubtitle1(),
            new Text("Subtitle 2").StyleAsSubtitle2(),
            new Text("Body 1").StyleAsBody1(),
            new Text("Body 2").StyleAsBody2(),
            new Text("Caption").StyleAsBody2(),
            new Text("OVERLINE").StyleAsOverline(),
        };
    }
}
