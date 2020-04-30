using System;
using System.Maui;

namespace NewApp
{
    public class MainPage : View
    {
        public MainPage()
        {
            this.Title("Welcome to Maui");
        }
        readonly State<int> count = 0;
        [Body]
        View body() => new NavigationView
        {
            new VStack
            {
                new Spacer(),
                new Label(() => $"Value: {count.Value}")
                    .Color(Color.Black)
                    .TextAlignment(TextAlignment.Left)
                    .FontSize(32),
                new Spacer(),
                new Button("+", () => count.Value ++ ).StyleAsCircleButton(),				
            }
        };
	}
}
