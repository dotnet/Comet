using System;
namespace Comet
{
	public class ReadonlyRequiresException : Exception
	{
		public ReadonlyRequiresException(string className, string propertyName) : base($"{className}.{propertyName} is not readonly")
		{
			ClassName = className;
			PropertyName = propertyName;
		}

		public string ClassName { get; }
		public string PropertyName { get; }
	}
}
