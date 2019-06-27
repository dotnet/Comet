using System;
using System.Collections.Generic;
using HotUI.Samples.Models;

namespace HotUI.Samples {
	public class ListPage : View {
		//This should come from a database or something
		List<Song> Songs = new List<Song> {
			new Song {
				Title = "All the Small Things",
				Artist = "Blink-182",
				Album = "Greatest Hits",
				ArtworkUrl = "http://lh3.googleusercontent.com/9Ofo9ZHQODFvahjpq2ZVUUOog4v5J1c4Gw9qjTw-KADTQZ6sG98GA1732mZA165RBoyxfoMblA"
			},
			new Song {
				Title = "Monster",
				Artist = "Skillet",
				Album = "Awake",
				ArtworkUrl = "http://lh3.googleusercontent.com/uhjRXO19CiZbT46srdXSM-lQ8xCsurU-xaVg6lvJvNy8TisdjlaHrHsBwcWAzpu_vkKXAA9SdbA",
			}
		};
		public ListPage()
		{
			Body = body;
		}

		View body ()  => new ListView<Song>(Songs) {
					Cell = (song) => new NavigationButton(()=> new ListViewDetails(song)){
						new Stack {
							new Image (song.ArtworkUrl),
							new Stack {
								new Text (song.Title),
								new Text (song.Artist),
								new Text (song.Album),
							}
						}
					},
				}.OnSelected ((song) => {
					Console.WriteLine ("Song Selected");
			});

	}
}
