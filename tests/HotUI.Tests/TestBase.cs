using System;
using Xunit;

[assembly: CollectionBehavior (DisableTestParallelization = true)]
namespace HotUI.Tests {
	public class TestBase {
		public TestBase ()
		{
			UI.Init ();
		}
	}
}
