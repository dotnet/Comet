using System.Diagnostics;

namespace Comet
{
    [DebuggerDisplay("Left={Left}, Top={Top}, Right={Right}, Bottom={Bottom}, HorizontalThickness={HorizontalThickness}, VerticalThickness={VerticalThickness}")]
    public struct Thickness
    {
        public static readonly Thickness Empty = new Thickness(0);
        
        public float Left { get; set; }

        public float Top { get; set; }

        public float Right { get; set; }

        public float Bottom { get; set; }

        public float HorizontalThickness => Left + Right;

        public float VerticalThickness => Top + Bottom;

        public bool IsEmpty => Left == 0 && Top == 0 && Right == 0 && Bottom == 0;

        public Thickness(float uniformSize) : this(uniformSize, uniformSize, uniformSize, uniformSize)
        {
        }

        public Thickness(float horizontalSize, float verticalSize) : this(horizontalSize, verticalSize, horizontalSize, verticalSize)
        {
        }

        public Thickness(float left, float top, float right, float bottom) : this()
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public static implicit operator Thickness(SizeF size)
        {
            return new Thickness(size.Width, size.Height, size.Width, size.Height);
        }

        public static implicit operator Thickness(float uniformSize)
        {
            return new Thickness(uniformSize);
        }

        bool Equals(Thickness other)
        {
            return Left.Equals(other.Left) && Top.Equals(other.Top) && Right.Equals(other.Right) && Bottom.Equals(other.Bottom);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Thickness && Equals((Thickness)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Left.GetHashCode();
                hashCode = (hashCode * 397) ^ Top.GetHashCode();
                hashCode = (hashCode * 397) ^ Right.GetHashCode();
                hashCode = (hashCode * 397) ^ Bottom.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Thickness left, Thickness right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Thickness left, Thickness right)
        {
            return !left.Equals(right);
        }

        public void Deconstruct(out float left, out float top, out float right, out float bottom)
        {
            left = Left;
            top = Top;
            right = Right;
            bottom = Bottom;
        }
    }
}