using System;
using System.Maui.Samples.Models;

namespace System.Maui.Samples
{
	public class ListViewDetails : View
	{
		[Environment]
		readonly Song song;

		public ListViewDetails()
		{
			Body = () => new VStack {
				new Image(() => song.ArtworkUrl),
				new Label(() => song.Title),
				new Label(() => song.Artist),
				new Label(() => song.Album),
			};
		}
	}
}
