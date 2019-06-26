using System;

namespace HotUI
{
    public class LayoutEventArgs : EventArgs
    {
        public int Start { get; }
        public int Count { get; }
        
        public LayoutEventArgs(int start, int count)
        {
            Start = start;
            Count = count;
        }
    }
}