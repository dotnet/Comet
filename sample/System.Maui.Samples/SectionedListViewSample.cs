using System;
using System.Linq;

namespace System.Maui.Samples
{
	public class SectionedListViewSample : View
	{
		public SectionedListViewSample()
		{
			int total = 10;
			var sections = Enumerable.Range(0, total).Select(s => new Section(header: new Label (s.ToString()))
			{
				Enumerable.Range(0, total).Select(r => new Label(r.ToString())),

			}).ToList();
			Body = () => new SectionedListView(sections);
		}
	}
}
