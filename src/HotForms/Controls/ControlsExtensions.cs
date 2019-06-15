using System;
using fLayoutOptions = Xamarin.Forms.LayoutOptions;
namespace HotForms {
	public static class ControlsExtensions {
		public static LayoutOptions Convert(this fLayoutOptions options)
		{
			if (options.Equals(fLayoutOptions.Center))
				return LayoutOptions.Center;
			if (options.Equals (fLayoutOptions.CenterAndExpand))
				return LayoutOptions.CenterAndExpand;
			if (options.Equals (fLayoutOptions.End))
				return LayoutOptions.End;
			if (options.Equals (fLayoutOptions.EndAndExpand))
				return LayoutOptions.EndAndExpand;
			if (options.Equals (fLayoutOptions.Fill))
				return LayoutOptions.Fill;
			if (options.Equals (fLayoutOptions.FillAndExpand))
				return LayoutOptions.FillAndExpand;
			if (options.Equals (fLayoutOptions.Start))
				return LayoutOptions.Start;
			if (options.Equals (fLayoutOptions.StartAndExpand))
				return LayoutOptions.StartAndExpand;
			throw new NotSupportedException ();
		}

		public static fLayoutOptions Convert (this LayoutOptions options)
		{
			if (options == LayoutOptions.Center)
				return fLayoutOptions.Center;
			if (options == LayoutOptions.CenterAndExpand)
				return fLayoutOptions.CenterAndExpand;
			if (options == LayoutOptions.End)
				return fLayoutOptions.End;
			if (options == LayoutOptions.EndAndExpand)
				return fLayoutOptions.EndAndExpand;
			if (options == LayoutOptions.Fill)
				return fLayoutOptions.Fill;
			if (options == LayoutOptions.FillAndExpand)
				return fLayoutOptions.FillAndExpand;
			if (options == LayoutOptions.Start)
				return fLayoutOptions.Start;
			if (options == LayoutOptions.StartAndExpand)
				return fLayoutOptions.StartAndExpand;
			throw new NotSupportedException ();
		}

		static bool IsSame(this fLayoutOptions options, fLayoutOptions otheroptions)
			=> options.Alignment == otheroptions.Alignment && options.Expands == otheroptions.Expands;
	}
}
