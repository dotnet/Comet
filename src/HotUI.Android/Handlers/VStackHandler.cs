using System;
using System.Collections.Generic;
using Android.Graphics;
using Android.Widget;
using AView = Android.Views.View;

namespace HotUI.Android
{
    public class VStackHandler : LinearLayout, IView
    {
        public VStackHandler() : base(AndroidContext.CurrentContext)
        {
            Orientation = Orientation.Vertical;
        }
        
        public AView View => this;
        public object NativeView => View;
        public bool HasContainer { get; set; } = false;

        public void Remove(View view)
        {
        }

        VStack stack;

        public void SetView(View view)
        {
            stack = view as VStack;
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

        protected void UpdateChildren(VStack stack)
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
                var cview = child.ToView();
                views.Add(cview);
                //cview.ContentMode = UIViewContentMode.Top;
                base.AddView(cview);
            }
        }
    }
}