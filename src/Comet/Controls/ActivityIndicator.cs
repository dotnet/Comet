using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet
{
	public class ActivityIndicator : View, IActivityIndicator
	{
		public ActivityIndicator()
		{

		}

		bool IActivityIndicator.IsRunning => true;

		Color IActivityIndicator.Color => this.GetColor();
	}
}
