using System;
using System.Diagnostics;
using System.Globalization;

namespace HotUI
{
    [DebuggerDisplay("X={X}, Y={Y}")]
    public struct Point
    {
        public float X { get; set; }

        public float Y { get; set; }

        public static Point Zero = new Point();

        public override string ToString()
        {
            return $"{{X={X.ToString(CultureInfo.InvariantCulture)} Y={Y.ToString(CultureInfo.InvariantCulture)}}}";
        }

        public Point(float x, float y) : this()
        {
            X = x;
            Y = y;
        }

        public Point(Size sz) : this()
        {
            X = sz.Width;
            Y = sz.Height;
        }

        public override bool Equals(object o)
        {
            if (!(o is Point))
                return false;

            return this == (Point)o;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ (Y.GetHashCode() * 397);
        }

        public Point Offset(float dx, float dy)
        {
            Point p = this;
            p.X += dx;
            p.Y += dy;
            return p;
        }

        public Point Round()
        {
            return new Point((float)Math.Round(X), (float)Math.Round(Y));
        }

        public bool IsEmpty => X == 0 && Y == 0;

        public static explicit operator Size(Point pt)
        {
            return new Size(pt.X, pt.Y);
        }

        public static Point operator +(Point pt, Size sz)
        {
            return new Point(pt.X + sz.Width, pt.Y + sz.Height);
        }

        public static Point operator -(Point pt, Size sz)
        {
            return new Point(pt.X - sz.Width, pt.Y - sz.Height);
        }

        public static bool operator ==(Point ptA, Point ptB)
        {
            return ptA.X == ptB.X && ptA.Y == ptB.Y;
        }

        public static bool operator !=(Point ptA, Point ptB)
        {
            return ptA.X != ptB.X || ptA.Y != ptB.Y;
        }

        public float Distance(Point other)
        {
            return (float)Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }

        public void Deconstruct(out float x, out float y)
        {
            x = X;
            y = Y;
        }
    }
}