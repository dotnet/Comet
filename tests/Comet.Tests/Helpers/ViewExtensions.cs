using System;
using System.Reflection;

namespace Comet.Tests
{
    public static class ViewExtensions
    {
        public static View GetReplacedView(this View view)
        {
            var field = typeof(View).GetField("replacedView", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            return (View)field.GetValue(view);
        }
    }
}
