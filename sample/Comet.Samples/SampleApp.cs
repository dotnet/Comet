using System;
namespace Comet.Samples
{
	public class SampleApp : CometApp
	{
		public SampleApp()
		{
			Body = () => new MainPage();
		}
	}
}
