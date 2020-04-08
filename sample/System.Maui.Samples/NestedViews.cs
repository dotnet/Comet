using System;
namespace System.Maui.Samples
{
	public class NestedViews : View
	{
		public NestedViews()
		{
			Body = () => new View
			{
				Body = () => new View
				{
					Body = () => new Text("Hi!")
				}
			};
		}
	}
}