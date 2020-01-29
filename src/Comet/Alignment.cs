namespace Comet
{
	public class Alignment
	{
		public static readonly Alignment Bottom = new Alignment(HorizontalAlignment.Center, VerticalAlignment.Bottom);
		public static readonly Alignment BottomLeading = new Alignment(HorizontalAlignment.Leading, VerticalAlignment.Bottom);
		public static readonly Alignment BottomTrailing = new Alignment(HorizontalAlignment.Trailing, VerticalAlignment.Bottom);
		public static readonly Alignment Center = new Alignment(HorizontalAlignment.Center, VerticalAlignment.Center);
		public static readonly Alignment Leading = new Alignment(HorizontalAlignment.Leading, VerticalAlignment.Center);
		public static readonly Alignment Trailing = new Alignment(HorizontalAlignment.Trailing, VerticalAlignment.Center);
		public static readonly Alignment Top = new Alignment(HorizontalAlignment.Center, VerticalAlignment.Top);
		public static readonly Alignment TopLeading = new Alignment(HorizontalAlignment.Leading, VerticalAlignment.Top);
		public static readonly Alignment TopTrailing = new Alignment(HorizontalAlignment.Trailing, VerticalAlignment.Top);

		public Alignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
		{
			Horizontal = horizontal;
			Vertical = vertical;
		}

		public HorizontalAlignment Horizontal { get; }
		public VerticalAlignment Vertical { get; }

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
