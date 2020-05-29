using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Comet.SourceGenerators
{
	/// <summary>
	/// Collection of things to help working with Roslyn
	/// </summary>
	public static class RoslynHelpers
	{
		// Copied from @terrajobst: https://github.com/terrajobst/minsk/blob/master/src/Minsk.Generators/SyntaxNodeGetChildrenGenerator.cs#L141-L154
		public static bool IsDerivedFrom(ITypeSymbol type, INamedTypeSymbol baseType)
		{
			var current = type;

			while (current != null)
			{
				if (SymbolEqualityComparer.Default.Equals(current, baseType))
					return true;

				current = current.BaseType;
			}

			return false;
		}
	}
}
