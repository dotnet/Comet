using System;
using HotUI.Samples.Models;

namespace HotUI.Samples {
	public class ListViewDetails : View {
		[State]
		Song song;
		public ListViewDetails (Song song)
		{
			this.song = song;
			Body = () => new Stack {
				new Image(song.ArtworkUrl),
				new Text(song.Title),
				new Text(song.Artist),
				new Text(song.Album),
			};
		}
	}
}
