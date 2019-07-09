using System;
using CoreGraphics;

namespace HotUI.iOS.Controls
{
    public class SizeChangedEventArgs : EventArgs
    {
        public SizeChangedEventArgs(CGSize size)
        {
            Size = size;
        }

        public CGSize Size { get;  }
        
    }
}