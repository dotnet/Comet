using System;
namespace Comet.Samples
{
    public class VirtualListViewSample : View
    {
        public VirtualListViewSample()
        {
            Body = () => new ListView<int>
            {
                Count= ()=> 10,
            };
        }
    }
}
