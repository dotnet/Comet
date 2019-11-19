using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SkiaSharp;

namespace Comet.Skia
{
    public class SkiaView : View
    {
        public event Action Invalidated;
        
        private readonly List<string> _boundProperties = new List<string>();
        
        protected SkiaView() : this(new PropertyMapper<SkiaView>())
        {
            
        }
        
        protected SkiaView(PropertyMapper<SkiaView> mapper)
        {
            Mapper = mapper ?? new PropertyMapper<SkiaView>();
        }

        internal BindingState GetState() => State;
        
        public PropertyMapper<SkiaView> Mapper { get; }
    
        public RectangleF Bounds { get; private set; }
        
        public void Invalidate()
        {
            Invalidated?.Invoke();
        }

        public virtual void Draw(SKCanvas canvas, RectangleF dirtyRect)
        {
            
        }

        public virtual void StartHoverInteraction(PointF[] points)
        {
        }

        public virtual void HoverInteraction(PointF[] points)
        {
        }

        public virtual void EndHoverInteraction()
        {
        }

        public virtual bool StartInteraction(PointF[] points)
        {
            return false;
        }

        public virtual void DragInteraction(PointF[] points)
        {
            
        }

        public virtual void EndInteraction(PointF[] points, bool inside)
        {
            
        }

        public virtual void CancelInteraction()
        {
            
        }

        public virtual void Resized(RectangleF bounds)
        {
            Bounds = bounds;
        }
        
        public override void ViewPropertyChanged(string property, object value)
        {
            base.ViewPropertyChanged(property, value);
            if (_boundProperties.Contains(property))
                Invalidate();
        }
        
        protected void SetBindingValue<T>(
            ref Binding<T> currentValue, 
            Binding<T> newValue, 
            [CallerMemberName] string propertyName = "")
        {
            currentValue = newValue;
            newValue?.BindToProperty(this, propertyName);
            _boundProperties.Add(propertyName);
        }
    }
}