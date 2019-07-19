using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SkiaSharp;

namespace HotUI.Skia
{
    public abstract class AbstractControlDelegate : IControlDelegate
    {
        public event Action Invalidated;
        
        private readonly Dictionary<string, BindingDefinition> _bindings = new Dictionary<string, BindingDefinition>();

        protected AbstractControlDelegate()
        {
            Mapper = new PropertyMapper<DrawableControl>();
        }
        
        protected AbstractControlDelegate(PropertyMapper<DrawableControl> mapper)
        {
            Mapper = mapper ?? new PropertyMapper<DrawableControl>();
        }

        public IReadOnlyDictionary<string, BindingDefinition> Bindings => _bindings;
        
        protected void Bind<T>(
            Binding<T> binding, 
            string propertyName,
            Action<T> updater)
        {
            if (binding == null) return;
            _bindings[propertyName] = new BindingDefinition(binding, propertyName, v => updater.Invoke((T)v));
        }
        
        public PropertyMapper<DrawableControl> Mapper { get; }
    
        public RectangleF Bounds { get; private set; }
        
        public void Invalidate()
        {
            Invalidated?.Invoke();
        }

        public abstract void Draw(SKCanvas canvas, RectangleF dirtyRect);

        public virtual bool StartInteraction(PointF[] points)
        {
            return false;
        }

        public virtual void DragInteraction(PointF[] points)
        {
            
        }

        public virtual void EndInteraction(PointF[] points)
        {
            
        }

        public virtual void CancelInteraction()
        {
            
        }

        public virtual void Resized(RectangleF bounds)
        {
            Bounds = bounds;
        }

        public virtual void AddedToView(object view, RectangleF bounds)
        {
            Bounds = bounds;
        }

        public void RemovedFromView(object view)
        {
            
        }

        public DrawableControl VirtualDrawableControl { get; set; }
            
        public IDrawableControl NativeDrawableControl { get; set; }
        
        public virtual SizeF Measure(SizeF availableSize)
        {
            return availableSize;
        }

        public static implicit operator View(AbstractControlDelegate controlDelegate)
        {
            return new DrawableControl(controlDelegate);
        }
        
        protected void SetValue<T> (ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            VirtualDrawableControl.SetStateValue (ref currentValue, newValue , propertyName);
        }
    }
}