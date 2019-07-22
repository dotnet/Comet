using System;
using System.Collections.Generic;
using System.Drawing;
using HotUI.Samples.Models;

namespace HotUI.Samples
{
    public class ListViewSample2 : View
    {
        //This should come from a database or something
        List<Song> Songs = new List<Song>
        {
            new Song
            {
                Title = "All the Small Things",
                Artist = "Blink-182",
                Album = "Dude Ranch",
                ArtworkUrl = "http://lh3.googleusercontent.com/9Ofo9ZHQODFvahjpq2ZVUUOog4v5J1c4Gw9qjTw-KADTQZ6sG98GA1732mZA165RBoyxfoMblA"
            },
            new Song
            {
                Title = "Monster",
                Artist = "Skillet",
                Album = "Awake",
                ArtworkUrl = "http://lh3.googleusercontent.com/uhjRXO19CiZbT46srdXSM-lQ8xCsurU-xaVg6lvJvNy8TisdjlaHrHsBwcWAzpu_vkKXAA9SdbA",
            }
        };

		public ListViewSample2 ()
		{
			Body = () => new ListView<Song> (Songs) 
			{
				Cell = song => new HStack
				{
					new Image(song.ArtworkUrl)
						.Frame(44,44, Alignment.Center)
						.Padding(left:10f)
						.ClipShape(new Circle()),
					new VStack(HorizontalAlignment.Leading)
					{
						new Text(song.Title),
						new Text(song.Artist),
						new Text(song.Album),
					},
					new Spacer()
				},
				Header = new VStack
				{
					new Text("Songs")
				},
			}.OnSelected ((song) => { Console.WriteLine ("Song Selected"); });
		}

    }
}