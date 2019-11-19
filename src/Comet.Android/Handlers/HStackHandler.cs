using System;
using System.Collections.Generic;
using Android.Widget;
using Comet.Android.Controls;
using AView = Android.Views.View;
using AOrientation = Android.Widget.Orientation;

namespace Comet.Android.Handlers
{
    public class HStackHandler : LinearLayout, AndroidViewHandler
    {
        public event EventHandler<ViewChangedEventArgs> NativeViewChanged;

        public HStackHandler() : base(AndroidContext.CurrentContext)
        {
            Orientation = AOrientation.Horizontal;
        }

        
        public AView View => this;
        public object NativeView => View;
        public bool HasContainer { get; set; } = false;

        public CUITouchGestureListener GestureListener { get; set; }

        public SizeF Measure(SizeF availableSize)
        {
            return availableSize;
        }

        public void SetFrame(RectangleF frame)
        {
            // Do nothing
        }

        public void Remove(View view)
        {
        }

        HStack stack;

        public void SetView(View view)
        {
            stack = view as HStack;
            UpdateChildren(stack);
            stack.ChildrenChanged += Stack_ChildrenChanged;
        }

        private void Stack_ChildrenChanged(object sender, EventArgs e)
        {
            UpdateChildren(stack);
        }

        public void UpdateValue(string property, object value)
        {
        }

        readonly List<AView> views = new List<AView>();

        protected void UpdateChildren(HStack stack)
        {
            var children = stack.GetChildren();
            if (views.Count == children.Count)
            {
                bool areSame = false;
                for (var i = 0; i < views.Count; i++)
                {
                    var v = views[i];
                    var c = children[i].ToView();
                    areSame = c == v;
                    if (!areSame)
                    {
                        break;
                    }
                }

                if (areSame)
                    return;
            }

            foreach (var v in views)
            {
                base.RemoveView(v);
            }

            views.Clear();
            foreach (var child in children)
            {
                var cview = child.ToView() ?? new AView(AndroidContext.CurrentContext);
                views.Add(cview);
                //cview.ContentMode = UIViewContentMode.Top;
                base.AddView(cview);
            }
        }
    }
}
