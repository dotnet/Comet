using System;
using System.Collections.Generic;
using HotUI.Samples.Models;

namespace HotUI.Samples
{
    public class ListViewSample1 : View
    {
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

        [Body]
        View body() => new ListView<Song>(Songs)
        {
            Cell = (song) => new HStack {
                            new Image (song.ArtworkUrl).Frame(52, 52).Padding(4),
                            new VStack(HorizontalAlignment.Leading, spacing:2)
                            {
                                new Text (song.Title).Font(Font.System(17)),
                                new Text (song.Artist).Color(Color.Grey),
                                new Text (song.Album).Color(Color.Grey),
                            }.Font(Font.System(12))

             }.Frame(height: 60),
        }.Navigate((song) => new ListViewDetails(song));
    }
}
