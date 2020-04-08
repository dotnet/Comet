using System;

namespace System.Maui.Graphics
{
	public class Gradient
	{
		private Stop[] _sortedStops;

		/// <summary>
		/// The gradient synthesizes its location values to evenly space the colors along the gradient.
		/// </summary>
		/// <param name="colors"></param>
		public Gradient(Color[] colors)
		{
			if (colors == null) throw new ArgumentNullException(nameof(colors));

			Stops = new Stop[colors.Length];
			for (var i = 0; i < colors.Length; i++)
			{
				var offset = (float)i / (float)(colors.Length - 1);
				Stops[i] = new Stop(offset, colors[i]);
			}
		}

		/// <summary>
		/// Creates a gradient from an array of color stops.
		/// </summary>
		/// <param name="stops"></param>
		public Gradient(Stop[] stops)
		{
			if (stops == null) throw new ArgumentNullException(nameof(stops));

			Stops = stops;
		}

		public Stop[] Stops { get; }

		public Stop[] GetSortedStops()
		{
			if (_sortedStops == null)
			{
				_sortedStops = new Stop[Stops.Length];
				Array.Copy(Stops, _sortedStops, Stops.Length);
				Array.Sort(_sortedStops);
			}

			return _sortedStops;
		}
	}
}
