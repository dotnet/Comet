using System;
namespace HotUI {
	public static class Extensions {
		public static string FirstCharToUpper (this string input)
			=> string.IsNullOrWhiteSpace (input) ? input[0].ToString ().ToUpper () + input.Substring (1)
			: input;
	}
}
