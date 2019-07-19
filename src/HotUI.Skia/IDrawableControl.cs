using System;
namespace HotUI.Skia
{
    public interface IDrawableControl
    {
        IControlDelegate ControlDelegate { get; set; }
    }
}
