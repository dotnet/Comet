using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Comet.Internal;
using SkiaSharp;
using Topten.RichTextKit;
using Comet.Skia.Internal;
namespace Comet.Skia
{
	public partial class FluentToggleHandler
	{
		public FluentToggleHandler() : base(ToggleDrawMapper, Mapper) { }
		public static DrawMapper<Toggle> ToggleDrawMapper = new DrawMapper<Toggle>(SkiaControl.DrawMapper)
		{
			["Background"] = MapDrawBackground,
			["Handle"] = MapDrawHandle,
		};
		public static string[] DefaultToggleLayerDrawingOrder = new []
		{
			SkiaEnvironmentKeys.Clip,
			"Background",
			"Handle",
			SkiaEnvironmentKeys.Overlay
		};
		protected override string[] LayerDrawingOrder() => DefaultToggleLayerDrawingOrder;
		static Dictionary<string, object> stateDefaultValues = new Dictionary<string, object>
		{
			["BackgroundColor.On"] = new Color(0f, 0.47058824f, 0.83137256f, 1f),
			["HandleFill1.On"] = new Color(1f, 1f, 1f, 1f),
			["HandleShadow1.On"] = new Color(0f, 0f, 0f, 0.15f),
			["HandleRect1.On"] = new RectangleF(21.5f, 1.5f,28f,28f),
			["BackgroundColor.Off"] = new Color(0.47058824f, 0.47058824f, 0.5019608f, 1f),
			["HandleFill1.Off"] = new Color(1f, 1f, 1f, 1f),
			["HandleShadow1.Off"] = new Color(0f, 0f, 0f, 0.15f),
			["HandleRect1.Off"] = new RectangleF(1.5f, 1.5f,28f,28f),
			["BackgroundColor.On (disabled)"] = new Color(0.7804f, 0.8784f, 0.9569f, 1f),
			["HandleFill1.On (disabled)"] = new Color(1f, 1f, 1f, 1f),
			["HandleRect1.On (disabled)"] = new RectangleF(21.5f, 1.5f,28f,28f),
			["BackgroundColor.Off (disabled)"] = new Color(0.47058824f, 0.47058824f, 0.5019608f, 1f),
			["HandleFill1.Off (disabled)"] = new Color(1f, 1f, 1f, 1f),
			["HandleRect1.Off (disabled)"] = new RectangleF(1.5f, 1.5f,28f,28f),
		};
		static string[] stateKeys = new []
		{
			"BackgroundColor",
			"HandleFill1",
			"HandleShadow1",
			"HandleRect1",
		};
		protected virtual void DrawBackground (SKCanvas canvas, RectangleF rectangle)
		{
			var bgRect = new RoundedRectangle(20);
			canvas.DrawShape(bgRect, rectangle, fill: this.GetEnvironment<Color>("BackgroundColor"));
			
		}
		protected virtual void DrawHandle (SKCanvas canvas, RectangleF rectangle)
		{
			{
				var p = new Comet.Path("M14 28C21.732 28 28 21.732 28 14C28 6.26801 21.732 0 14 0C6.26801 0 0 6.26801 0 14C0 21.732 6.26801 28 14 28Z");
				var fill = this.GetEnvironment<Color>("HandleFill1");
				Comet.Graphics.Shadow shadow = null;
				Color shadowColor = this.GetEnvironment<Color>("HandleShadow1");
				shadowColor = this.GetEnvironment<Color>("HandleShadow1");
				shadow = new Comet.Graphics.Shadow(shadowColor,new SizeF(0f,3f) ,8f,1);
				canvas.DrawShape(p, this.GetEnvironment<RectangleF>("HandleRect1"), fill: fill, fillShadow: shadow);
			}
			
		}
		static void MapDrawBackground (SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, Toggle view)
		{
			var typed = control as FluentToggleHandler;
			typed?.DrawBackground (canvas, dirtyRect);
		}
		static void MapDrawHandle (SKCanvas canvas, RectangleF dirtyRect, SkiaControl control, Toggle view)
		{
			var typed = control as FluentToggleHandler;
			typed?.DrawHandle (canvas, dirtyRect);
		}
	}
}
