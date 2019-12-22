using System;
using System.Collections.Generic;

namespace Comet
{
    public class LayoutEventArgs : EventArgs
    {
        public int Start { get; }
        public int Count { get; }
        public List<View> Removed { get; }

        public LayoutEventArgs(int start, int count)
        {
            Start = start;
            Count = count;
        }

        public LayoutEventArgs(int start, int count, List<View> removed)
        {
            Start = start;
            Count = count;
            Removed = removed;
        }
    }
}
