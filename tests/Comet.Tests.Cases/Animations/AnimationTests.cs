using System;
using Xunit;
using Microsoft.Maui.Animations;
using Microsoft.Maui;

namespace Comet.Tests.Cases
{
    public class AnimationTests : TestBase
    {
        [Fact]
		public void DoubleAnimatesOverTime()
        {
            var animation = new LerpingAnimation
            {
                StartValue = 0.0,
                EndValue = 1.0,
                Easing = Easing.Linear
            };

            var progress = .1;
            animation.Update (progress);
            Assert.Equal(animation.CurrentValue, progress);

            progress = .2;
            animation.Update (progress);
            Assert.Equal(animation.CurrentValue, progress);

            progress = .33333;
            animation.Update (progress);
            Assert.Equal(animation.CurrentValue, progress);

            progress = .75;
            animation.Update (progress);
            Assert.Equal(animation.CurrentValue, progress);

            progress = .99;
            animation.Update (progress);
            Assert.Equal(animation.CurrentValue, progress);

            progress = 1;
            animation.Update (progress);
            Assert.Equal(animation.CurrentValue, progress);
        }
    }
}
