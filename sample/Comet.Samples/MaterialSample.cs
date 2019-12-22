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
                new Button("Button").StyleAsContained(),
                new Button("Button").StyleAsOutlined(),
                new Button("Button").StyleAsText(),
            }
        };

    }
}
