using System;
using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers
{
    public class ScrollViewHandler : NSScrollView, INSView
    {
        public NSView View => this;

        public ScrollViewHandler()
        {
            Console.WriteLine("New scrollview handler created");
        }

        public void Remove(View view)
        {
            content?.RemoveFromSuperview();
        }

        NSView content;

        public void SetView(View view)
        {
            var scroll = view as ScrollView;

            content = scroll?.View?.ToView();
            if (content != null)
            {
                //todo: fix this.  This is a hack to get the content to show up in the scrollview
                if (content.Bounds.Width <= 0 && content.Bounds.Height <= 0)
                    content.Frame = new CoreGraphics.CGRect(0,0,800,600);
                 
                DocumentView = content;
            }
            this.UpdateProperties(view);
            NSLayoutConstraint.ActivateConstraints(
                new[]
                {
                    content.LeadingAnchor.ConstraintEqualToAnchor(this.LeadingAnchor),
                    content.TrailingAnchor.ConstraintEqualToAnchor(this.TrailingAnchor),
                    content.TopAnchor.ConstraintEqualToAnchor(this.TopAnchor),
                    content.BottomAnchor.ConstraintEqualToAnchor(this.BottomAnchor),
                });
        }

        public void UpdateValue(string property, object value)
        {
            this.UpdateProperty(property, value);
        }
    }
}