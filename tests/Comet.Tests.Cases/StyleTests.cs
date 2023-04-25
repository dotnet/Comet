using System.Buffers;
using Comet.Styles;
using Xunit;
using Microsoft.Maui.Graphics;
using Microsoft.Maui;
using Font = Microsoft.Maui.Font;

namespace Comet.Tests.Cases
{
	public class StyleTests : TestBase
	{
		public class TestButtonStyle : ButtonStyle
		{
			public TestButtonStyle()
			{
				TextColor = Colors.Red;
				Padding = new Thickness(5);
				TextFont = Font.SystemFontOfSize(10, FontWeight.Regular);
			}
		}

		[Fact]
		public void ButtonStyleTest()
		{
			var button = new Button("Test").Apply<TestButtonStyle>();
			Assert.Equal(((ITextStyle)button).TextColor, Colors.Red);
			Assert.Equal(((IPadding)button).Padding, new Thickness(5));
			Assert.Equal(((ITextStyle)button).Font, Font.SystemFontOfSize(10, FontWeight.Regular));
		}

		public class TestProgressBarStyle : ProgressBarStyle
		{
			public TestProgressBarStyle()
			{
				ProgressColor = Colors.Red;
			}
		}

		[Fact]
		public void ProgressBarStyleTest()
		{
			var progressBar = new ProgressBar().Apply<TestProgressBarStyle>();
			Assert.Equal(((IProgress)progressBar).ProgressColor, Colors.Red);
		}

		public class TestSliderStyle : SliderStyle
		{
			public TestSliderStyle()
			{
				ProgressColor = Colors.Red;
				TrackColor = Colors.Yellow;
				ThumbColor = Colors.Blue;
			}
		}

		[Fact]
		public void SliderStyleTest()
		{
			var slider = new Slider().Apply<TestSliderStyle>();
			Assert.Equal(((ISlider)slider).MaximumTrackColor, Colors.Red);
			Assert.Equal(((ISlider)slider).MinimumTrackColor, Colors.Yellow);
			Assert.Equal(((ISlider)slider).ThumbColor, Colors.Blue);
		}
	}
}