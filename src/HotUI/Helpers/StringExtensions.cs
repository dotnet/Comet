using System;
using System.Text;

namespace HotUI
{
    public static class StringExtensions
    {
        public static string Repeat(this string input, int count)
        {
            if (!string.IsNullOrEmpty(input))
            {
                StringBuilder builder = new StringBuilder(input.Length * count);

                for (int i = 0; i < count; i++) builder.Append(input);

                return builder.ToString();
            }

            return string.Empty;
        }
    }
}
