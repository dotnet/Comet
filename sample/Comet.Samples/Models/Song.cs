using System;
namespace Comet.Samples.Models {
	public class Song : BindingObject {
		public string Title {
			get => GetProperty<string> ();
			set => SetProperty (value);
		}

		public string Artist {
			get => GetProperty<string> ();
			set => SetProperty (value);
		}
		
		public string Album {
			get => GetProperty<string> ();
			set => SetProperty (value);
		}

		public int TrackNumber {
			get => GetProperty<int> ();
			set => SetProperty (value);
		}

		public string ArtworkUrl {			
			get => GetProperty<string> ();
			set => SetProperty (value);
		}
	}
}
