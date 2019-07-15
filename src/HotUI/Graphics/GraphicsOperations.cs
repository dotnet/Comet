using System;

namespace HotUI.Graphics
{
    public static class GraphicsOperations
    {
        public static PointF RotatePoint(PointF point, float angleInDegrees)
        {
            var radians = DegreesToRadians(angleInDegrees);

            var x = (float) (Math.Cos(radians) * point.X - Math.Sin(radians) * point.Y);
            var y = (float) (Math.Sin(radians) * point.X + Math.Cos(radians) * point.Y);

            return new PointF(x, y);
        }
        
        public static PointF RotatePoint(PointF center, PointF point, float angleInDegrees)
        {
            var radians = DegreesToRadians(angleInDegrees);
            var x = center.X + (float) (Math.Cos(radians) * (point.X - center.X) - Math.Sin(radians) * (point.Y - center.Y));
            var y = center.Y + (float) (Math.Sin(radians) * (point.X - center.X) + Math.Cos(radians) * (point.Y - center.Y));
            return new PointF(x, y);
        }
        
        public static float DegreesToRadians(float angleInDegrees)
        {
            return (float) Math.PI * angleInDegrees / 180;
        }

        public static double DegreesToRadians(double angleInDegrees)
        {
            return Math.PI * angleInDegrees / 180;
        }

        public static float RadiansToDegrees(float angleInRadians)
        {
            return angleInRadians * (180 / (float) Math.PI);
        }

        public static double RadiansToDegrees(double angleInRadians)
        {
            return angleInRadians * (180 / Math.PI);
        }
        
        public static float GetSweep(float angle1, float angle2, bool clockwise)
        {
            if (clockwise)
            {
                if (angle2 > angle1)
                    return angle1 + (360 - angle2);

                return angle1 - angle2;
            }

            if (angle1 > angle2)
                return angle2 + (360 - angle1);

            return angle2 - angle1;
        }
    }
}