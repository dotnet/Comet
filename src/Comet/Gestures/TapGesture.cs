using System;
namespace Comet
{
	public class TapGesture : Gesture<TapGesture>
	{
		public TapGesture(Action<TapGesture> action) : base(action)
		{

		}
	}
}
