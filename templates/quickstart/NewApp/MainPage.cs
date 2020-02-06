using System;
using Comet;

namespace NewApp
{
    public class MainPage : View
    {
        readonly State<int> count = 0;

        [Body]
        View body() => new VStack
        {
            new Text(() => $"Value: {count.Value}")
                .Color(Color.Black)
                .FontSize(32),
            new Button("Increment", () => count.Value ++ )
                .Frame(width:320, height:44)
                .Background(Color.Black)
                .Color(Color.White)
                .Padding(20)
            ,
        };
    }
}
