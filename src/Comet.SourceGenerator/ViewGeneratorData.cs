using System;
using System.Collections.Generic;
using System.Text;

namespace Comet.SourceGenerator
{
	public class ViewGeneratorData
	{
		public List<string> NameSpaces { get; set; } = new List<string>();
		public string NameSpace { get; set; }

		public string ClassName { get; set; }

		public string BaseClassName { get; set; }

		public List<(string Type, string Name, string DefaultValueString)> Parameters { get; set; } = new ();

		public List<(string Type, string Name, string FullName, bool IsMethod, bool ShouldBeExtension)> Properties { get; set; } = new();

		public List<string> PropertiesWithSet { get; set; } = new();
	}
}
