using System;
namespace Comet
{
	public interface IGestureView
	{
		IReadOnlyList<Gesture> Gestures { get; }
	}
}

