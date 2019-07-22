using System;
using HotUI.Layout;

// ReSharper disable once CheckNamespace
namespace HotUI
{
    public static class LayoutExtensions
    {
        /// <summary>
        /// Set the padding to the default thickness.
        /// </summary>
        /// <param name="view"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Padding<T>(this T view) where T : View
        {
            var defaultThickness = new Thickness(10);
            view.Padding = defaultThickness;
            return view;
        }
        
        public static T Padding<T>(this T view, float? left = null, float? top= null, float? right= null, float? bottom = null) where T : View
        {
            view.Padding = new Thickness(
                left ?? 0,
                top ?? 0,
                right ?? 0,
                bottom ?? 0);
            return view;
        }
        
        public static Thickness GetPadding(this View view)
        {
            return view.Padding;
            ;
        }

        public static T Overlay<T>(this T view, View overlayView) where T : View
        {
            view.SetEnvironment(EnvironmentKeys.View.Overlay, overlayView);
            return view;
        }

        public static T Overlay<T>(this T view, Shape shape) where T : View
        {
            var shapeView = new ShapeView(shape);
            view.SetEnvironment(EnvironmentKeys.View.Overlay, shapeView);
            return view;
        }

        public static View GetOverlay(this View view)
        {
            return view.GetEnvironment<View>(EnvironmentKeys.View.Overlay);
        }

        public static T Frame<T>(this T view, float? width = null, float? height = null, Alignment alignment = null) where T : View
        {
            view.FrameConstraints = new FrameConstraints(width, height, alignment);
            return view;
        }
        
        public static T Cell<T>(
            this T view,             
            int row = 0,
            int column = 0,
            int rowSpan = 1,
            int colSpan = 1,
            float weightX = 1,
            float weightY = 1,
            float positionX = 0,
            float positionY = 0) where T : View
        {
            view.LayoutConstraints = new GridConstraints(row, column, rowSpan, colSpan, weightX, weightY, positionX, positionY);
            return view;
        }

        public static void SetFrameFromNativeView(
            this View view,
            RectangleF frame)
        {
            var padding = view.GetPadding();
            if (!padding.IsEmpty)   
            {
                frame.X += padding.Left;
                frame.Y += padding.Top;
                frame.Width -= padding.HorizontalThickness;
                frame.Height -= padding.VerticalThickness;
            }

            var sizeThatFits = view.Measure(frame.Size);
            view.MeasuredSize = sizeThatFits;
            view.MeasurementValid = true;

            var width = sizeThatFits.Width;
            var height = sizeThatFits.Height;

            var frameConstraints = view.FrameConstraints;

            if (frameConstraints?.Width != null)
                width = (float)frameConstraints.Width;

            if (frameConstraints?.Height != null)
                height = (float)frameConstraints.Height;

            var alignment = frameConstraints?.Alignment ?? Alignment.Center;

            var xFactor = .5f;
            switch (alignment.Horizontal)
            {
                case HorizontalAlignment.Leading:
                    xFactor = 0;
                    break;
                case HorizontalAlignment.Trailing:
                    xFactor = 1;
                    break;
            }

            var yFactor = .5f;
            switch (alignment.Vertical)
            {
                case VerticalAlignment.Bottom:
                    yFactor = 1;
                    break;
                case VerticalAlignment.Top:
                    yFactor = 0;
                    break;
            }

            var x = frame.X + ((frame.Width - width) * xFactor);
            var y = frame.Y + ((frame.Height - height) * yFactor);
            view.Frame = new RectangleF((float)x, (float)y, width, height);
        }
    }
}