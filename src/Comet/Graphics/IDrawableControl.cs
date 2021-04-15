using System;
namespace Comet.Graphics
{
	public interface IDrawableControl
	{
		IControlDelegate ControlDelegate { get; set; }
	}
}
