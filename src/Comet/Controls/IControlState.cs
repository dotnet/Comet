using System;
namespace Comet
{
	public interface IControlState
	{
		ControlState CurrentState { get; set; }
		Action<ControlState> StateChanged { get; }
	}
}
