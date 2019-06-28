using System;
using System.Collections.Generic;
using System.Drawing;
using HotUI.Samples.Models;

namespace HotUI.Samples
{
    public class ListPage2 : View
    {
        //This should come from a database or something
        List<Song> Songs = new List<Song>
        {
            new Song
            {
                Title = "All the Small Things",
                Artist = "Blink-182",
                Album = "Dude Ranch",
            },
            new Song
            {
                Title = "Monster",
                Artist = "Skillet",
                Album = "Awake",
            }
        };

		public ListPage2 ()
		{
			Body = () => new ListView<Song> (Songs) {
				Cell = song => new HStack
					{
					new Image(song.ArtworkUrl),
					new VStack
					{
						new Text(song.Title),
						new Text(song.Artist),
						new Text(song.Album),
					}
				},
				Header = group => new VStack
					{
					new Text(group.ToString())
				},
			}.OnSelected ((song) => { Console.WriteLine ("Song Selected"); });
		}

    }
}