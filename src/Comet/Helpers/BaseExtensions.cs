using System;

// ReSharper disable once CheckNamespace
namespace Comet
{
    public static class BaseExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            if (input != null)
                return string.IsNullOrWhiteSpace(input)
                    ? input[0].ToString().ToUpper() + input.Substring(1)
                    : input;

            return null;
        }
    }
}
