using System;
using AppKit;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Controls
{
    public class HUITableViewCell : NSTableCellView
    {
        private NSView currentContent;
        private View currentView;

        public void SetView(View view)
        {
            //TODO:We should do View Compare
            //view.Diff (view);
            currentContent?.RemoveFromSuperview();
            currentContent = view.ToView();
            currentView = view;
            Console.WriteLine(currentContent.FittingSize);
            this.AddSubview(currentContent);
        }

        public override void Layout()
        {
            base.Layout();
            if (currentContent == null)
                return;
            currentContent.Frame = Bounds;
        }
    }
}
