using System;
namespace System.Maui.Samples
{
	public class VirtualListViewSample : View
	{
		public VirtualListViewSample()
		{
			Body = () => new ListView<int>
			{
				Count = () => 10,
				ItemFor = (i) => i,
				ViewFor = (i) => new Label (i.ToString()),
			};
		}
	}
}
