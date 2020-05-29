using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

		internal static bool ImplementsInterface(ITypeSymbol typeSymbol, INamedTypeSymbol interfaceType)
		{
			return typeSymbol.AllInterfaces.Any(iface => SymbolEqualityComparer.Default.Equals(iface, interfaceType));
		}
	}
}
