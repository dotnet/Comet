using System;
using System.Collections.Generic;
using System.Text;

static class StringExtensions
{
	public static string LowercaseFirst(this string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			return value;
		var cleanValue = value.Trim();
		var first = char.ToLower(cleanValue[0]);
		return $"{first}{cleanValue.Substring(1)}";
	}


}


namespace Comet.SourceGenerator
{
	public partial class ViewGenerator
	{
		

	}
}