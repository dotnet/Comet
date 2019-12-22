using System;
using Comet.Samples.Models;

namespace Comet.Samples
{
	public class ListViewDetails : View
	{
		[State]
		readonly Song song;
		public ListViewDetails(Song song)
		{
			this.song = song;
			Body = () => new VStack {
				new Image(() => song.ArtworkUrl),
				new Text(() => song.Title),
				new Text(() => song.Artist),
				new Text(() => song.Album),
			};
		}
	}
}
