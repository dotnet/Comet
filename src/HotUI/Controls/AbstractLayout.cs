using System;
using System.Collections;
using System.Collections.Generic;
using HotUI.Layout;

namespace HotUI
{
	public abstract class AbstractLayout : ContainerView
    {
		private readonly ILayoutManager _layout;
		
		protected AbstractLayout(ILayoutManager layoutManager)
		{
			_layout = layoutManager;
		}

		public ILayoutManager LayoutManager => _layout;

        protected override void OnAdded(View view)
        {
            _layout?.Invalidate();
        }

        protected override void OnClear()
        {
            _layout?.Invalidate();
        }

        protected override void OnRemoved(View view)
        {
            _layout?.Invalidate();
        }

        protected override void OnInsert(int index, View item)
        {
            _layout?.Invalidate();
        }
        


        public override SizeF Measure(SizeF availableSize)
        {
	        var width = FrameConstraints?.Width;
	        var height = FrameConstraints?.Height;

	        // If we have both width and height constraints, we can skip measuring the control and
	        // return the constrained values.
	        if (width != null && height != null)
		        return new SizeF((float)width, (float)height);

	        var measuredSize = _layout?.Measure(this, availableSize) ?? availableSize;
            
	        // If we have a constraint for just one of the values, then combine the constrained value
	        // with the measured value for our size.
	        if (width != null || height != null)
		        return new SizeF(width ?? measuredSize.Width, height ?? measuredSize.Height);

	        return measuredSize;
        }
        
        public override void LayoutSubviews(RectangleF bounds)
        {
	        _layout?.Layout(this, bounds.Size);
        }
    
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _layout.Invalidate();
        }
	}
}
