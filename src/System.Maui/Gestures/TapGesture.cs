using System;
namespace System.Maui
{
	public class TapGesture : Gesture<TapGesture>
	{
		public TapGesture(Action<TapGesture> action) : base(action)
		{

		}
	}
}
