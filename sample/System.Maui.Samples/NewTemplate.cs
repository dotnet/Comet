using System;
namespace System.Maui.Samples
{
	public class NewTemplate : View
	{
		public NewTemplate()
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
					.TextAlignment(TextAlignment.Center)
					.FontSize(32),
				new Spacer(),
				new Button("+", () => count.Value ++ )
				//.StyleAsCircleButton(),				
			}
		};
	}
}
