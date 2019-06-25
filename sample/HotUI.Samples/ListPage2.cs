using System;
using System.Collections.Generic;
using System.Drawing;
using HotUI.Samples.Models;

namespace HotUI.Samples
{
    public class ListPage2 : HotPage
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

        protected override View Build() => new ListView<Song>(Songs)
        {
            Cell = song => new Stack
                {
                    new Image(song.ArtworkUrl),
                    new Stack
                    {
                        new Text(song.Title),
                        new Text(song.Artist),
                        new Text(song.Album),
                    }
                },
            Header = group => new Stack
                {
                    new Text(group)
                },
            BackgroundColor = Color.Green,
        }.OnSelected((song) => { Console.WriteLine("Song Selected"); });
    }
}