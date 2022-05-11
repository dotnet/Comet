using System;
namespace Comet.Samples
{
	public class VirtualSectionedListViewSample : View
	{
		public VirtualSectionedListViewSample()
		{
			Body = () => new SectionedListView<int>
			{
				SectionCount = () => 10,
				SectionFor = (s) => new Section<int>
				{
					Header = new Text($"Header: {s}"),
					Count = () => 10,
					ItemFor = (index) => index,
					ViewFor = (i) => new Text($"Row: {i}"),
				},
			};
		}
	}
}
