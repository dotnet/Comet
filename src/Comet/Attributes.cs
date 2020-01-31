using System;

namespace Comet
{
	[AttributeUsage(AttributeTargets.Field)]
	public class StateAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class BodyAttribute : Attribute
	{
	}
}
