using System;
using System.Collections.Generic;
using AppKit;
using CoreGraphics;
using HotUI.Mac.Controls;
using HotUI.Mac.Extensions;

namespace HotUI.Mac.Handlers
{
    public class VStackHandler : AbstractLayoutHandler
    {
        public VStackHandler(CGRect rect) : base(rect, new VStackLayoutManager<UIView>())
        {
        }

        public VStackHandler() : base(new VStackLayoutManager<UIView>())
        {
        }
    }
}