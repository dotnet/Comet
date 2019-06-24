using System;
using System.Collections.Generic;
using AppKit;
using CoreGraphics;
using HotUI.Mac.Controls;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers
{
    public class StackHandler : StackView, INSView
    {
        public StackHandler()
        {
            Frame = new CGRect(0, 0, 800, 600);
            Console.WriteLine("New stack handler created");
        }

        public NSView View => this;

        public void Remove(View view)
        {
            if (stack != null)
            {
                stack.ChildrenChanged -= Stack_ChildrenChanged;
                stack = null;
            }
        }

        Stack stack;

        public void SetView(View view)
        {
            stack = view as Stack;
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

        List<NSView> views = new List<NSView>();

        protected void UpdateChildren(Stack stack)
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
                v.RemoveFromSuperview();
            views.Clear();

            foreach (var child in children)
            {
                var cview = child.ToView();
                cview.TranslatesAutoresizingMaskIntoConstraints = false;

                views.Add(cview);

                // todo: fixme, this is hack to get the controls to show up
                if (cview.Bounds.Width <= 0 && cview.Bounds.Height <= 0)
                    cview.SetFrameSize(new CGSize(200, 24));

                AddSubview(cview);
            }

            LayoutSubviews();
        }
    }
}