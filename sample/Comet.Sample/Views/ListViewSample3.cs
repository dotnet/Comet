using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Comet.Samples.Models;
using Microsoft.Maui.Graphics;

namespace Comet.Samples
{
	public class ListViewSample3 : View
	{
		//This should come from a database or something
		ObservableCollection<Song> Songs = new ObservableCollection<Song> {
			new Song {
				Title = "All the Small Things",
				Artist = "Blink-182",
				Album = "Greatest Hits",
				TrackNumber = 1,
				ArtworkUrl = "https://picsum.photos/100"
			},
			new Song {
				Title = "Monster",
				Artist = "Skillet",
				Album = "Awake",
				TrackNumber = 2,
				ArtworkUrl = "https://picsum.photos/100",
			}
		};

		[Body]
		View body() => new ListView<Song>(Songs)
		{
			ViewFor = (song) => new HStack
			{
				new Image (song.ArtworkUrl).Frame(100, 100).Margin(4),
				new VStack(LayoutAlignment.Start, spacing:2)
				{
					new Text (song.Title).FontSize(17),
					new Text (song.Artist).Color(Colors.Grey),
					new Text (song.Album).Color(Colors.Grey),
					new HStack
					{
						new Text(() => $"Current track number is {song.TrackNumber}"),
						new Button("Increase tracker number", ()=> song.TrackNumber++)
					}
				}.FontSize(12),
				new Button("Remove", ()=> Songs.Remove(song))
			}.Frame(height: 100).Alignment(Alignment.Leading),
		}.OnSelectedNavigate((song) => new ListViewDetails().SetEnvironment("song", song));
	}
	/*
	In XAML if we want to binding a command in Listview item, 
	we need to create a command in cell's viewmodel. 
	And it will more complex to talk with page's viewmodel
	*/
}