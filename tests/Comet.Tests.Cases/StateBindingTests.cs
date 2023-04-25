using System;
using Comet.Internal;
using Xunit;

namespace Comet.Tests.Cases
{
	public class StateBindingTests : TestBase
	{
		public class MyDataModel : BindingObject
		{
			public int ChildCount
			{
				get => GetProperty<int>(10);
				set => SetProperty(value);
			}
			public int ParentCount
			{
				get => GetProperty<int>(10);
				set => SetProperty(value);
			}
		}

		public class ParentView : View
		{
			[State]
			public readonly MyDataModel model = new MyDataModel();
		}

		public class ChildView : View
		{
			[Environment]
			public readonly MyDataModel model;
		}


		[Fact(Skip = "Needs Fixing")]
		public void GlobalBindingsOnlyRefreshTheViewThatHasTheGlobal()
		{
			int parentBodyBuildCount = 0;
			int childBodyBuildCount = 0;

			ParentView parentView = null;
			parentView = new ParentView
			{
				Body = () => {
					Console.WriteLine(parentView.model.ParentCount);
					parentBodyBuildCount++;
					return new ChildView()
					{
						Body = () => {
							Console.WriteLine(parentView.model.ChildCount);
							childBodyBuildCount++;
							return new Text(childBodyBuildCount.ToString());
						}
					}.SetEnvironment("model", parentView.model);
				}
			};

			InitializeHandlers(parentView);
			Assert.False(StateManager.IsBuilding);
			var parentGlobalState = parentView.InternalGetState().GlobalProperties;
			Assert.Single(parentGlobalState);


			var childGlobalState = parentView.BuiltView.InternalGetState().GlobalProperties.Count;
			Assert.Equal(1, parentBodyBuildCount);
			Assert.Equal(1, childBodyBuildCount);

			parentView.model.ParentCount++;
			Assert.False(StateManager.IsBuilding);

			Assert.Equal(2, parentBodyBuildCount);
			Assert.Equal(2, childBodyBuildCount);
			parentView.model.ChildCount++;
			Assert.False(StateManager.IsBuilding);

			Assert.Equal(2, parentBodyBuildCount);
			Assert.Equal(3, childBodyBuildCount);


		}

		[Fact]
		public void BuildingChildren()
		{
			ParentView parent = null;
			parent = new ParentView
			{
				Body = () => new View
				{
					Body = () => new Text($"Child Click Count: {parent.model.ChildCount}")
				}
			};
		}
	}
}
