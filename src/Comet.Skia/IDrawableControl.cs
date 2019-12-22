using System;
namespace Comet.Skia
{
    public interface IDrawableControl
    {
        IControlDelegate ControlDelegate { get; set; }
    }
}
