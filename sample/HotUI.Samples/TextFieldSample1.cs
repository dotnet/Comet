using System;
using System.Collections.Generic;

namespace HotUI.Samples
{
    public class TextFieldSample1 : View
    {
        readonly State<string> name1 = "";
        readonly State<string> name2 = "";

        [Body]
        View body() => new VStack
        {
            new TextField(null, "Name", name1, name2),
            new HStack()
            {
                new Text("onEditingChanged:"),
                new Text(name1),
                new Spacer()
            },
            new HStack()
            {
                new Text("onCommit:"),
                new Text(name2),
                new Spacer()
            },
        };
    }

}