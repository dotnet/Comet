using System;
using Microsoft.Maui;
using Xunit;

namespace Comet.Tests.Cases
{
	public class MapperTests : TestBase
	{


		[Fact]
		public void ChainingMappersOverrideBase()
		{
			bool wasMapper1Called = false;
			bool wasMapper2Called = false;
			var mapper1 = new PropertyMapper<View>
			{
				[EnvironmentKeys.Colors.Background] = (r, v) => wasMapper1Called = true
			};

			var mapper2 = new PropertyMapper<Button>(mapper1)
			{
				[EnvironmentKeys.Colors.Background] = (r, v) => wasMapper2Called = true
			};

			mapper2.UpdateProperties(null, new Button());

			Assert.False(wasMapper1Called);
			Assert.True(wasMapper2Called);
		}

		[Fact]
		public void ChainingMappersWorks()
		{
			bool wasMapper1Called = false;
			bool wasMapper2Called = false;
			var mapper1 = new PropertyMapper<View>
			{
				[EnvironmentKeys.Colors.Color] = (r, v) => wasMapper1Called = true
			};

			var mapper2 = new PropertyMapper<Button>(mapper1)
			{
				[EnvironmentKeys.Colors.Background] = (r, v) => wasMapper2Called = true
			};

			mapper2.UpdateProperties(null, new Button());

			Assert.True(wasMapper1Called);
			Assert.True(wasMapper2Called);
		}

		[Fact]
		public void ChainingMappersStillAllowReplacingChainedRoot()
		{
			bool wasMapper1Called = false;
			bool wasMapper2Called = false;
			bool wasMapper3Called = false;
			var mapper1 = new PropertyMapper<View>
			{
				[EnvironmentKeys.Colors.Color] = (r, v) => wasMapper1Called = true
			};

			var mapper2 = new PropertyMapper<Button>(mapper1)
			{
				[EnvironmentKeys.Colors.Background] = (r, v) => wasMapper2Called = true
			};

			mapper1[EnvironmentKeys.Colors.Color] = (r, v) => wasMapper3Called = true;

			mapper2.UpdateProperties(null, new Button());

			Assert.False(wasMapper1Called,"Mapper 1 was called");
			Assert.True(wasMapper2Called, "Mapper 2 was called");
			Assert.True(wasMapper3Called, "Mapper 3 was called");
		}
	}
}
