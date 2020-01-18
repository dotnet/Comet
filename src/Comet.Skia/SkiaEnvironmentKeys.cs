using System;
namespace Comet.Skia
{
	public static class SkiaEnvironmentKeys
	{
		//public const string Draw = "Skia.Draw";
		public const string Clip = "Skia.Clip";
		public const string Text = "Skia.Text";
		public const string Background = "Skia.Background";
		public const string Border = "Skia.Border";
		public const string Overlay = "Skia.Overlay";
		public static class IntrinsicSize
		{
			public const string Height = "Skia.IntrinsicSize.Height";
			public const string Width = "Skia.IntrinsicSize.Width";
		}
		public static class Slider
		{
			public static class Layers
			{
				public const string Track = "Skia.Slider.Layers.Track";
				public const string Progress = "Skia.Slider.Layers.Progress";
				public const string Thumb = "Skia.Slider.Layers.Thumb";
			}
		}

		public static class Toggle
		{
			public static class Layers
			{
				public const string Track = "Skia.Toggle.Layers.Track";
				public const string Thumb = "Skia.Toggle.Layers.Thumb";
			}
		}
	}
}
