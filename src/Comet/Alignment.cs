using Microsoft.Maui.Primitives;

namespace Comet
{
	public class Alignment
	{
		public static readonly Alignment Bottom = new Alignment(LayoutAlignment.Center, LayoutAlignment.End);
		public static readonly Alignment BottomLeading = new Alignment(LayoutAlignment.Start, LayoutAlignment.End);
		public static readonly Alignment BottomTrailing = new Alignment(LayoutAlignment.End, LayoutAlignment.End);
		public static readonly Alignment Center = new Alignment(LayoutAlignment.Center, LayoutAlignment.Center);
		public static readonly Alignment Leading = new Alignment(LayoutAlignment.Start, LayoutAlignment.Center);
		public static readonly Alignment Trailing = new Alignment(LayoutAlignment.End, LayoutAlignment.Center);
		public static readonly Alignment Top = new Alignment(LayoutAlignment.Center, LayoutAlignment.Start);
		public static readonly Alignment TopLeading = new Alignment(LayoutAlignment.Start, LayoutAlignment.Start);
		public static readonly Alignment TopTrailing = new Alignment(LayoutAlignment.End, LayoutAlignment.Start);
		public static readonly Alignment Fill = new Alignment(LayoutAlignment.Fill, LayoutAlignment.Fill);

		public Alignment(LayoutAlignment horizontal, LayoutAlignment vertical)
		{
			Horizontal = horizontal;
			Vertical = vertical;
		}

		public LayoutAlignment Horizontal { get; }
		public LayoutAlignment Vertical { get; }

		protected bool Equals(Alignment other) => Horizontal == other.Horizontal && Vertical == other.Vertical;

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((Alignment)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((int)Horizontal * 397) ^ (int)Vertical;
			}
		}
	}
}
