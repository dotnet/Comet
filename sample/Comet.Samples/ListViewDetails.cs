using System;
using Comet.Samples.Models;

namespace Comet.Samples
{
	public class ListViewDetails : View
	{
		[Environment]
		readonly Song song;

		public ListViewDetails()
		{
			Body = () => new VStack {
				new Image(() => song.ArtworkUrl),
				new Text(() => song.Title),
				new Text(() => song.Artist),
				new Text(() => song.Album),
			};
		}
	}
}
