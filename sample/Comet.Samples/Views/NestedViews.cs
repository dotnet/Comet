using System;
namespace Comet.Samples
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