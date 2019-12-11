using System;
using Xunit;

namespace Comet.Tests
{
    public class AnimationTests : TestBase
    {
        [Fact]
		public void DoubleAnimatesOverTime()
        {
            var animation = new Animation
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
