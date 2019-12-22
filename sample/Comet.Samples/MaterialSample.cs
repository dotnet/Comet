using Comet.Styles.Material;
using System;
using System.Collections.Generic;
using System.Text;

namespace Comet.Samples
{
	public class MaterialSample : View
	{
		[Body]
		View body() => new VStack
		{
			new HStack
			{
				new Button("Contained Button").StyleAsContained(),
				new Button("Outlined Button").StyleAsOutlined(),
				new Button("Text Button").StyleAsText(),
			}
		};

	}
}
