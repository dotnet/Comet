using System;
using Comet.Internal;
using Microsoft.Maui.HotReload;
using Xunit;

namespace Comet.Tests.Cases
{
	public class HotReloadWithParameters : TestBase
	{
		public class MyOrgView : View
		{
			public const string TextValue = "Hello!";
			public MyOrgView(string text)
			{
				MauiHotReloadHelper.Register(this, text);
				this.Body = () => new Text(text);
			}
		}
		public class MyNewView : View
		{
			public MyNewView(string text)
			{
				MauiHotReloadHelper.Register(this, text);
				this.Body = () => new Text(text);
			}
		}

		public class MyOrgView1 : View
		{
			public const string TextValue = "Hello!";
			public MyOrgView1(string text = TextValue)
			{
				MauiHotReloadHelper.Register(this, text);
				this.Body = () => new Text(text);
			}
		}
		public class MyNewView1 : View
		{
			public const string TextValue = "Hello!";
			public MyNewView1(string text = TextValue)
			{
				MauiHotReloadHelper.Register(this, text);
				this.Body = () => new Text(text);
			}
		}

		public HotReloadWithParameters()
		{
			MauiHotReloadHelper.IsEnabled = true;
		}

		[Fact]
		public void HotReloadRegisterReplacedViewReplacesView()
		{
			var orgView = new MyOrgView(MyOrgView.TextValue);
			var orgText = orgView.GetView() as Text;
			Assert.Equal(MyOrgView.TextValue, orgText.Value);

			MauiHotReloadHelper.RegisterReplacedView(typeof(MyOrgView).FullName, typeof(MyNewView));
			var newText = orgView.GetView() as Text;

			Assert.Equal(MyOrgView.TextValue, newText.Value);

		}

		[Fact]
		public void HotReloadRegisterReplacedViewReplacesViewWithDefaultParameters()
		{
			var orgView = new MyOrgView1(MyOrgView.TextValue);
			var orgText = orgView.GetView() as Text;
			Assert.Equal(MyOrgView1.TextValue, orgText.Value);

			MauiHotReloadHelper.RegisterReplacedView(typeof(MyOrgView1).FullName, typeof(MyNewView));
			var newText = orgView.GetView() as Text;

			Assert.Equal(MyOrgView1.TextValue, newText.Value);

		}
	}
}
